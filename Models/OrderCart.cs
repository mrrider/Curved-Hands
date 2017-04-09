using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TTP_Project.Models.entities;
using TTP_Project.Models.repository;

namespace TTP_Project.Models
{
    public class OrderCart{

        UnitOfWork unitOfWork = new UnitOfWork();

        string OrderCartId { get; set; }

        public const string CartSessionKey = "CartId";

        public static OrderCart GetCart(HttpContextBase context)
        {
            var cart = new OrderCart();
            cart.OrderCartId = cart.GetCartId(context);
            return cart;
        }
        
        // Helper method to simplify shopping cart calls
        public static OrderCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }
        public void AddToCart(ProductItem productItem)
        {
            // Get the matching cart and album instances
            var cartItem = unitOfWork.CartRepository.Get().ToList<Cart>().SingleOrDefault(
                c => c.CartId == OrderCartId 
                && c.ProductItemId == productItem.ID);
 
            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new Cart
                {
                    ProductItemId = productItem.ID,
                    CartId = OrderCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
               unitOfWork.CartRepository.Insert(cartItem);
            }
            else
            {
                // If the item does exist in the cart, 
                // then add one to the quantity
                cartItem.Count++;
            }
            // Save changes
            unitOfWork.Save();
        }
        public int RemoveFromCart(int id)
        {
            // Get the cart
            var cartItem = unitOfWork.CartRepository.Get().ToList<Cart>().Single(
                cart => cart.CartId == OrderCartId 
                && cart.RecordId == id);
 
            int itemCount = 0;
 
            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    unitOfWork.CartRepository.Delete(cartItem);
                }
                // Save changes
                unitOfWork.Save();
            }
            return itemCount;
        }
        public void EmptyCart()
        {
            var cartItems =unitOfWork.CartRepository.dbSet.Where(
                cart => cart.CartId == OrderCartId);
 
            foreach (var cartItem in cartItems)
            {
                unitOfWork.CartRepository.Delete(cartItem);
            }
            // Save changes
            unitOfWork.Save();
        }
        public List<Cart> GetCartItems()
        {
            return unitOfWork.CartRepository.dbSet.Where(
                cart => cart.CartId == OrderCartId).ToList();
        }
        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in unitOfWork.CartRepository.dbSet
                          where cartItems.CartId == OrderCartId
                          select (int?)cartItems.Count).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }
        public decimal GetTotal()
        {
            // Multiply album price by count of that album to get 
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total
            decimal? total = (from cartItems in unitOfWork.CartRepository.dbSet
                              where cartItems.CartId == OrderCartId
                              select (int?)cartItems.Count *
                              cartItems.ProductItem.Price).Sum();

            return total ?? decimal.Zero;
        }

        public int CreateOrder(Order order)
        {
            order.Total = this.GetTotal();
            string itemList = "";
            foreach (Cart item in this.GetCartItems())
            {
                itemList += item.ProductItemId + ":" + item.Count + ";";
            }
            order.orderItemsIds = itemList;
            unitOfWork.Save();
            EmptyCart();
            return order.OrderId;
        }

        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] =
                        context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }
        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string userName)
        {
            var OrderCart = unitOfWork.CartRepository.dbSet.Where(
                c => c.CartId == OrderCartId);
 
            foreach (Cart item in OrderCart)
            {
                item.CartId = userName;
            }
            unitOfWork.Save();
        }
    }
}
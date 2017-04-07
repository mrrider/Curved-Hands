using TTP_Project.Models;
using TTP_Project.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TTP_Project.Models.entities;
using TTP_Project.Models.repository;

namespace TTP_Project.Controllers
{
    public class ShoppingCartController : Controller
    {
        UnitOfWork unityOfWork = new UnitOfWork();
     
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            return View(viewModel);
        }
        
       
        [HttpPost]
        public ActionResult AddToCart(int id)
        {
            var addedItem = unityOfWork.ProductItemRepository.dbSet
                .Single(item => item.ID == id);
            
            var cart = ShoppingCart.GetCart(this.HttpContext);

            int count = cart.AddToCart(addedItem);
            
            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(addedItem.Name) +
                    " has been added to your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = count,
                DeleteId = id
            };
            return Json(results);
            
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            string itemName = unityOfWork.ProductItemRepository.dbSet
                .Single(item => item.ID == id).Name;
            
            int itemCount = cart.RemoveFromCart(id);
            
            var results = new ShoppingCartRemoveViewModel
            {
                Message = "One (1) " + Server.HtmlEncode(itemName) +
                    " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }

        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}
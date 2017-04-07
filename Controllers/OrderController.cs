using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TTP_Project.Models;
using TTP_Project.Models.entities;
using TTP_Project.Models.repository;
using TTP_Project.Models.ViewModels;

namespace TTP_Project.Controllers
{
    public class OrderController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork();
      
       public ActionResult Items()
        {
            IEnumerable<ProductItem> items = unitOfWork.ProductItemRepository.Get().ToList<ProductItem>();
            return View(items);
        }
        
        public ActionResult Index()
        {
            var cart = OrderCart.GetCart(this.HttpContext);
 
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };

            return View(viewModel);
        }
 
        public ActionResult AddToCart(int id)
        {
            var addedAlbum = unitOfWork.ProductItemRepository.dbSet
                .Single(album => album.ID == id);
 
            var cart = OrderCart.GetCart(this.HttpContext);
 
            cart.AddToCart(addedAlbum);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Order order)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.OrderRepository.Insert(order);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(order);
        }
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            var cart = OrderCart.GetCart(this.HttpContext);
 
            string albumName = unitOfWork.CartRepository.dbSet
                .Single(item => item.RecordId == id).ProductItem.Name;
 
            int itemCount = cart.RemoveFromCart(id);
 
            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(albumName) +
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
            var cart = OrderCart.GetCart(this.HttpContext);
 
            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }

}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TTP_Project.Models;
using TTP_Project.Models.constants;
using TTP_Project.Models.entities;
using TTP_Project.Models.repository;

namespace TTP_Project.Controllers
{
    public class CheckoutController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Customer")]
        public ActionResult AddressAndPayment()
        {
            return View();
        }
        
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var order = new Order();
            TryUpdateModel(order);

            order.OrderDate = DateTime.Now;
            order.completeDate = DateTime.Now.AddDays(7).Date;
            order.orderStartus = OrderStatus.Initial;
            order.detailDescription = values[0];
            order.orderItemsIds = new Dictionary<int,int>();  
            order.customer = unitOfWork.UserRepository.dbSet.Where(s => s.UserName.Equals(User.Identity.Name)).First(); 
            //There we send new customer order to order-operator
            
            var cart = OrderCart.GetCart(this);
            cart.CreateOrder(order);
            unitOfWork.OrderRepository.Insert(order);
            unitOfWork.Save();

            return RedirectToAction("Complete", new { id = order.OrderId });
                   
        }
        
        public ActionResult Complete(int id)
        {
            bool isValid = unitOfWork.OrderRepository.dbSet.Any(o => o.OrderId == id && o.customer.UserName == User.Identity.Name);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}
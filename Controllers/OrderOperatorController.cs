﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TTP_Project.Models.entities;
using TTP_Project.Models;
using TTP_Project.Models.repository;
using TTP_Project.Models.constants;

namespace TTP_Project.Controllers
{
    public class OrderOperatorController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UnitOfWork unitOfWork = new UnitOfWork();
        private IEnumerable<Order> activeOrders;
        
        public ActionResult Index()
        {
            activeOrders  = unitOfWork.OrderRepository.Get().Where(s => s.orderStartus.Equals(OrderStatus.Initiating));
            return View(activeOrders);
        }

        public ActionResult Reject(int? id)
        {
            Order ord = unitOfWork.OrderRepository.GetByID(id);
            ord.orderStartus = OrderStatus.Rejected;
            unitOfWork.OrderRepository.Update(ord);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }

        public ActionResult Confrim(int? id)
        {

            IEnumerable<ApplicationUser> them = unitOfWork.UserRepository.Get().Where(s => s.RoleName.Equals(RolesConst.MANAGER));

            ViewBag.pm = them;
            Project proj = new Project();
           
            return View(proj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confrim(Project pro)
        {
            if (ModelState.IsValid)
            {
                Order ord = unitOfWork.OrderRepository.GetByID(pro.id);

                ord.orderStartus = OrderStatus.Processiong;
                unitOfWork.OrderRepository.Update(ord);
               
                pro.order = ord;
                pro.costs = ord.Total;
                IEnumerable<ApplicationUser> them =  unitOfWork.UserRepository.Get().Where(s => s.RoleName.Equals(RolesConst.MANAGER));
                foreach (ApplicationUser manager in them)
                {
                    if (manager.UserName.Equals(pro.nameProjectManager))
                        pro.projectManager = manager;
                }
              
                pro.projectStatus = ProjectStatus.Initial;

                unitOfWork.ProjectRepository.Insert(pro);
                unitOfWork.Save();

                return RedirectToAction("Index");
            }
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
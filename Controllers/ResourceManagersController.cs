using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TTP_Project.Models;
using TTP_Project.Models.entities;
using TTP_Project.Models.repository;

namespace TTP_Project.Controllers
{
    public class ResourceManagersController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork();

        public ActionResult Index()
        {
            IEnumerable<Resourse> resourceItems = unitOfWork.ResourceRepository.Get().ToList<Resourse>();
            return View(resourceItems);
        }
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resourse resource = unitOfWork.ResourceRepository.GetByID(id);
            if (resource == null)
            {
                return HttpNotFound();
            }
            return View(resource);
        }
        
        public ActionResult Create()
        {
            ViewBag.resourses = unitOfWork.ResourceRepository.Get().ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Resourse resourceItem)
        {
            
                unitOfWork.ResourceRepository.Insert(resourceItem);
                unitOfWork.Save();

                return RedirectToAction("Index", "ResourceManagers");
        }
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resourse resourceItem = unitOfWork.ResourceRepository.GetByID(id);
            if (resourceItem == null)
            {
                return HttpNotFound();
            }
            return View(resourceItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Resourse resourceItem)
        {
            if (ModelState.IsValid)
            {
               unitOfWork.ResourceRepository.Update(resourceItem);
               unitOfWork.Save();
               
                return RedirectToAction("Index");
            }
            return View(resourceItem);
        }
        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resourse resource = unitOfWork.ResourceRepository.GetByID(id);
            if (resource == null)
            {
                return HttpNotFound();
            }
            return View(resource);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Resourse resource = unitOfWork.ResourceRepository.GetByID(id);
            unitOfWork.ResourceRepository.Delete(resource);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }

    
    }
}

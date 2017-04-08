using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TTP_Project.Models.entities;
using TTP_Project.Models.repository;
using TTP_Project.Models.ViewModels;

namespace TTP_Project.Controllers
{
    public class OperatorController : Controller
    {
        
        UnitOfWork unitOfWork = new UnitOfWork();

        [Authorize(Roles = "Operator")] 
        public ActionResult Index()
        {
            IEnumerable<WorkItem> workItems = unitOfWork.WorkItemRepository.Get().ToList();
            return View(workItems);
        }

        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkItem workItem = unitOfWork.WorkItemRepository.GetByID(id);
            if (workItem == null)
            {
                return HttpNotFound();
            }
            return View();
        }
       
        public ActionResult Create()
        {
            ViewBag.workItems = unitOfWork.WorkItemRepository.Get().ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WorkItemViewModel model)
        {
                var @workItem = new WorkItem() {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    DueDate = model.DueDate,
                    Status = model.Status,
                    AssignedWorker = model.AssignedWorker,
                    assignedProject = model.AssignedProject
                };

                unitOfWork.WorkItemRepository.Insert(@workItem);
                unitOfWork.Save();

                return RedirectToAction("Index", "Operator");
        }
        
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkItem @workItem = unitOfWork.WorkItemRepository.GetByID(id);
            if (@workItem == null)
            {
                return HttpNotFound();
            }
            return View(new WorkItemViewModel(@workItem));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WorkItemViewModel model)
        {
            WorkItem workItem = unitOfWork.WorkItemRepository.GetByID(model.Id);
            workItem.Name = model.Name;
            workItem.Description = model.Description;
            workItem.DueDate = model.DueDate;
            workItem.Status = model.Status;
            workItem.AssignedWorker = model.AssignedWorker;
            workItem.assignedProject = model.AssignedProject;

            unitOfWork.WorkItemRepository.Insert(workItem);
            unitOfWork.Save();
            
            return RedirectToAction("Index");
        }
        
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkItem @workItem;
            try
            {
                @workItem = unitOfWork.WorkItemRepository.GetByID(id);
            }catch(Exception ex) {
                @workItem = null;
                return HttpNotFound(ex.Message);
            }
                
            if (@workItem == null)
            {
                return HttpNotFound();
            }
            return View(@workItem);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WorkItem @workItem = unitOfWork.WorkItemRepository.GetByID(id);
            unitOfWork.WorkItemRepository.Delete(@workItem);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }

    
    }
}

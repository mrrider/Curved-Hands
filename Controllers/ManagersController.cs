using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TTP_Project.Models;
using TTP_Project.Models.constants;
using TTP_Project.Models.entities;
using TTP_Project.Models.repository;
using TTP_Project.Models.ViewModels;

namespace TTP_Project.Controllers
{
    public class ManagersController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork();

        [Authorize(Roles = "Managers")] 
        public ActionResult Index()
        {
            IEnumerable<Project> items = unitOfWork.ProjectRepository.Get().Where(s => s.projectManager.UserName.Equals(User.Identity.Name));
            return View(items);
        }
         [Authorize(Roles = "Managers")] 

        public ActionResult Details(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project item = unitOfWork.ProjectRepository.GetByID(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        [Authorize(Roles = "Managers")] 
        public ActionResult Edit(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.ps = (IEnumerable<ProjectStatus>) Enum.GetValues(typeof (ProjectStatus));
            Project item = unitOfWork.ProjectRepository.GetByID(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(new ProjectViewModel(item));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Managers")] 
        public ActionResult Edit(ProjectViewModel model)
        {
            Project item = unitOfWork.ProjectRepository.GetByID(model.id);
            item.projectStatus = model.projectStatus;

            unitOfWork.ProjectRepository.Update(item);
            unitOfWork.Save();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Managers")] 
        public ActionResult EditTask(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkItem item = unitOfWork.WorkItemRepository.GetByID(id);
            IEnumerable<ApplicationUser> them = unitOfWork.UserRepository.Get().Where(s => s.RoleName.Equals(RolesConst.PROGRAMER));
            
            ViewBag.programmers = them;

            if (item == null)
            {
                return HttpNotFound();
            }
            return View(new WorkItemViewModel(item));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Managers")] 
        public ActionResult EditTask(WorkItemViewModel model)
        {

            string newAssignWorkerUsername = Request.Form["AssignedWorker"].ToString();
            ApplicationUser newAssignUser = unitOfWork.UserRepository.Get().Where(s => s.UserName.Equals(newAssignWorkerUsername)).SingleOrDefault();
            WorkItem item = unitOfWork.WorkItemRepository.GetByID(model.Id);

            item.AssignedWorker= newAssignUser;
            unitOfWork.WorkItemRepository.Update(item);
            unitOfWork.Save();

            return RedirectToAction("Index");
        }

    }
}

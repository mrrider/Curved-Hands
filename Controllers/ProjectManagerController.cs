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
    [Authorize(Roles = "ProjectManager")]
    public class ProjectManagerController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork();

        [Authorize(Roles = "ProjectManager")] 
        public ActionResult Index()
        {
            IEnumerable<Project> items = unitOfWork.ProjectRepository.Get().Where(s => s.projectManager.UserName.Equals(User.Identity.Name));
            return View(items);
        }

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

            IEnumerable<WorkItem> wkItems = unitOfWork.WorkItemRepository.Get().Where(s => s.assignedProject.id == item.id);
            //Order ord = item.order;

            //List<WorkItem> wkItems = new List<WorkItem>();
            //string listItems = ord.orderItemsIds;
            //IDictionary<int, int> prItems = new Dictionary<int, int>();

            //string[] wkitem = listItems.Split(';');
            //foreach (string k in wkitem)
            //{

            //    string[] u = k.Split(':');
            //    if (u.Length > 1)
            //    {

            //        int key = int.Parse(u[0]);
            //        int value = int.Parse(u[1]);
            //        prItems.Add(key, value);
            //    }
            //}
            //foreach (KeyValuePair<int, int> kvp in prItems)
            //{
            //    ProductItem pr = unitOfWork.ProductItemRepository.GetByID(kvp.Key);
            //    List<WorkItem> wk = Utilts.GenericTasks(pr.Categorie).ToList();
            //    for (int i = 0; i < kvp.Value; i++)
            //        wkItems.AddRange(wk);
            //}

            item.tasks = wkItems.ToList();

            return View(item);
        }
        
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
        public ActionResult Edit(ProjectViewModel model)
        {
            Project item = unitOfWork.ProjectRepository.GetByID(model.id);
            bool canStart = true;

           foreach(WorkItem w in item.tasks)
            {
                if (w.AssignedWorker == null)
                    canStart = false;
            }

            if (canStart)
            {
                item.projectStatus = model.projectStatus;

                unitOfWork.ProjectRepository.Update(item);
                unitOfWork.Save();

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("About");
            }
        }
        
        public ActionResult EditTask(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkItem item = unitOfWork.WorkItemRepository.GetByID(id);
            IEnumerable<ApplicationUser> them = unitOfWork.UserRepository.Get().Where(s => s.RoleName.Equals(RolesConst.DEVELOPER));
            
            ViewBag.programmers = them;

            if (item == null)
            {
                return HttpNotFound();
            }
            return View(new WorkItemViewModel(item));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

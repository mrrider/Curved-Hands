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

        [Authorize(Roles = "ProjectManager")]
        public ActionResult Error()
        {
            return View();
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
            ProjectStatus oldstatus = item.projectStatus;
            foreach(WorkItem w in item.tasks)
            {
                if (w.AssignedWorker == null)
                    canStart = false;
            }

            if (canStart)
            {
                if (oldstatus == ProjectStatus.Initial && model.projectStatus == ProjectStatus.InProgress)
                {
                    item.projectStatus = model.projectStatus;

                    foreach (WorkItem w in item.tasks)
                    {
                        Finance last = unitOfWork.FinancesRepository.Get().Last();

                        Finance fin = new Finance()
                        {
                            TransactionName = "salary",
                            From = "company",
                            To = w.AssignedWorker.UserName,
                            ItemDescription = "advance",
                            Date = DateTime.Now,
                            Cost = 0 - w.Price/2,
                            Balance = last.Balance - w.Price/2
                        };

                        unitOfWork.FinancesRepository.Insert(fin);
                        unitOfWork.Save();
                    }

                    Finance last1 = unitOfWork.FinancesRepository.Get().Last();
                    decimal cost = 0 - item.order.Total * 0.05m;
                    Finance fin1 = new Finance()
                    {
                        TransactionName = "salary",
                        From = "company",
                        To = "projectManager",
                        ItemDescription = "advance",
                        Date = DateTime.Now,
                        Cost = cost,
                        Balance = last1.Balance + cost
                    };

                    unitOfWork.FinancesRepository.Insert(fin1);


                    unitOfWork.ProjectRepository.Update(item);
                    unitOfWork.Save();

                    return RedirectToAction("Index");
                }else if(oldstatus == ProjectStatus.InProgress && model.projectStatus == ProjectStatus.Completed)
                {
                    bool canClose = true;
                    foreach (WorkItem w in item.tasks)
                    {
                        if (w.Status != TaskStatus.Completed)
                            canClose = false;
                    }
                    if (canClose)
                    {
                        item.projectStatus = model.projectStatus;

                        unitOfWork.ProjectRepository.Update(item);
                        unitOfWork.Save();


                        //sell all salary

                        Finance last = unitOfWork.FinancesRepository.Get().Last();
                        decimal cost = 0 - item.order.Total * 0.05m;
                        Finance fin = new Finance()
                        {
                            TransactionName = "salary",
                            From = "company",
                            To = "projectManager",
                            ItemDescription = "salary",
                            Date = DateTime.Now,
                            Cost = cost,
                            Balance = last.Balance + cost
                        };

                        unitOfWork.FinancesRepository.Insert(fin);
                        unitOfWork.Save();

                        Finance last1 = unitOfWork.FinancesRepository.Get().Last();
                        Finance fin1 = new Finance()
                        {
                            TransactionName = "salary",
                            From = "company",
                            To = "orderManager",
                            ItemDescription = "salary",
                            Date = DateTime.Now,
                            Cost = cost,
                            Balance = last1.Balance + cost
                        };

                        unitOfWork.FinancesRepository.Insert(fin1);
                        unitOfWork.Save();



                        Finance last2 = unitOfWork.FinancesRepository.Get().Last();
                        cost = 0 - item.order.Total * 0.1m;
                        Finance fin2 = new Finance()
                        {
                            TransactionName = "salary",
                            From = "company",
                            To = "admin",
                            ItemDescription = "salary",
                            Date = DateTime.Now,
                            Cost = cost,
                            Balance = last2.Balance + cost
                        };

                        unitOfWork.FinancesRepository.Insert(fin2);
                        unitOfWork.Save();

                        Finance last3 = unitOfWork.FinancesRepository.Get().Last();
                        Finance fin3 = new Finance()
                        {
                            TransactionName = "salary",
                            From = "company",
                            To = "accountManager",
                            ItemDescription = "salary",
                            Date = DateTime.Now,
                            Cost = cost,
                            Balance = last3.Balance + cost
                        };

                        unitOfWork.FinancesRepository.Insert(fin3);
                        unitOfWork.Save();

                        foreach(WorkItem w in item.tasks)
                        {
                            Finance lastDev = unitOfWork.FinancesRepository.Get().Last();

                            Finance findev = new Finance()
                            {
                                TransactionName = "salary",
                                From = "company",
                                To = w.AssignedWorker.UserName,
                                ItemDescription = "salary",
                                Date = DateTime.Now,
                                Cost = 0 - w.Price / 2,
                                Balance = lastDev.Balance - w.Price / 2
                            };

                            unitOfWork.FinancesRepository.Insert(findev);
                            unitOfWork.Save();
                        }

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("Error");
                    }

                }
                else
                {
                    return RedirectToAction("Error");
                }
            }
            else
            {
                return RedirectToAction("Error");
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

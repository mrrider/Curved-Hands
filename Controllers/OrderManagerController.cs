using System;
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
    public class OrderManagerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UnitOfWork unitOfWork = new UnitOfWork();
        private IEnumerable<Order> activeOrders;
        
        public ActionResult Index()
        {
            activeOrders  = unitOfWork.OrderRepository.Get().Where(s => s.OrderStatus.Equals(OrderStatus.Initial));
            return View(activeOrders);
        }

        public ActionResult Reject(int? id)
        {
            Order ord = unitOfWork.OrderRepository.GetByID(id);
            ord.OrderStatus = OrderStatus.Rejected;
            unitOfWork.OrderRepository.Update(ord);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }

        public ActionResult Confrim(int? id)
        {

            IEnumerable<ApplicationUser> them = unitOfWork.UserRepository.Get().Where(s => s.RoleName.Equals(RolesConst.PROJECT_MANAGER));
            Order ord = unitOfWork.OrderRepository.GetByID(id);

            ViewBag.pm = them;
            Project proj = new Project();
            proj.costs = ord.Total;
            proj.projectStatus = ProjectStatus.Initial;
            proj.order = ord;
            List<WorkItem> wkItems = new List<WorkItem>();
            string listItems = ord.OrderItemsIds;
            IDictionary<int, int> prItems = new Dictionary<int, int>();

            string[] wkitem = listItems.Split(';');
            foreach(string k in wkitem)
            {
                
                    string[] u = k.Split(':');
                    if (u.Length > 1)
                    {
                        
                        int key = int.Parse(u[0]);
                        int value = int.Parse(u[1]);
                        prItems.Add(key, value);
                    }
            }
            foreach(KeyValuePair<int, int> kvp in prItems)
            {
                ProductItem pr = unitOfWork.ProductItemRepository.GetByID(kvp.Key);
                List<WorkItem> wk = Utilts.GenericTasks(pr.Categorie).ToList();
                for(int i = 0; i < kvp.Value; i++)
                    wkItems.AddRange(wk);
            }

            proj.tasks = wkItems;
           
            return View(proj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confrim(Project pro)
        {
            if (ModelState.IsValid)
            {
                Order ord = unitOfWork.OrderRepository.GetByID(pro.id);

                pro.order = ord;
                pro.costs = ord.Total;
                IEnumerable<ApplicationUser> them =  unitOfWork.UserRepository.Get().Where(s => s.RoleName.Equals(RolesConst.PROJECT_MANAGER));
                foreach (ApplicationUser manager in them)
                {
                    if (manager.UserName.Equals(pro.nameProjectManager))
                        pro.projectManager = manager;
                }
              
                pro.projectStatus = ProjectStatus.Initial;



                List<WorkItem> wkItems = new List<WorkItem>();
                string listItems = ord.OrderItemsIds;
                IDictionary<int, int> prItems = new Dictionary<int, int>();

                string[] wkitem = listItems.Split(';');
                foreach (string k in wkitem)
                {

                    string[] u = k.Split(':');
                    if (u.Length > 1)
                    {

                        int key = int.Parse(u[0]);
                        int value = int.Parse(u[1]);
                        prItems.Add(key, value);
                    }
                }
                foreach (KeyValuePair<int, int> kvp in prItems)
                {
                    ProductItem pr = unitOfWork.ProductItemRepository.GetByID(kvp.Key);
                    List<WorkItem> wk = Utilts.GenericTasks(pr.Categorie).ToList();
                    for (int i = 0; i < kvp.Value; i++)
                    {
                        foreach(WorkItem w in wk)
                        {
                            w.Description = pr.Description;
                        }
                        wkItems.AddRange(wk);
                    }
                }
                unitOfWork.ProjectRepository.Insert(pro);
                unitOfWork.Save();

                foreach (WorkItem wk in wkItems)
                {
                    wk.assignedProject = pro;
                    wk.DueDate = DateTime.Now.AddMonths(1).Date;
                    wk.DateCreated = DateTime.Now.Date;
                    unitOfWork.WorkItemRepository.Insert(wk);
                    unitOfWork.Save();

                }
                
                pro.tasks = wkItems;

                unitOfWork.ProjectRepository.Update(pro);
                ord.OrderStatus = OrderStatus.InProgress;
                unitOfWork.OrderRepository.Update(ord);

                Finance last = unitOfWork.FinancesRepository.Get().Last();
                decimal cost = 0 - ord.Total * 0.05m;
                Finance fin = new Finance()
                {
                    TransactionName = "salary",
                    From = "company",
                    To = "orderManager",
                    ItemDescription = "advance",
                    Date = DateTime.Now,
                    Cost = cost,
                    Balance = last.Balance + cost
                };

                unitOfWork.FinancesRepository.Insert(fin);

                unitOfWork.Save();


                Finance last1 = unitOfWork.FinancesRepository.Get().Last();
                Finance fin1 = new Finance()
                {
                    TransactionName = "income",
                    From = ord.Customer.UserName,
                    To = "company",
                    ItemDescription = "item_bought",
                    Date = DateTime.Now,
                    Cost = ord.Total,
                    Balance = last1.Balance + ord.Total
                };

                unitOfWork.FinancesRepository.Insert(fin1);

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

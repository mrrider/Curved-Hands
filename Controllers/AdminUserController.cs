using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using TTP_Project.Models;
using TTP_Project.Models.entities;
using TTP_Project.Models.repository;
using TTP_Project.Models.ViewModels;
using System.Collections.ObjectModel;
using TTP_Project.Models.constants;
using System.Data.Entity;
using PagedList;

namespace TTP_Project.Controllers
{
    public class AdminUserController : Controller
    {        
        private ApplicationUserManager _userManager;
        private UnitOfWork unityOfWork = new UnitOfWork();

        public AdminUserController()
        {
        }

        public AdminUserController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationDbContext _db = new ApplicationDbContext();

        [AllowAnonymous]
        [Authorize(Roles = "Admin")]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString,int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewBag.RoleSortParm = sortOrder == "Role" ? "Role_desc" : "Role";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var rolesList = new Collection<AdminUserViewModel>();
            var users = from s in _db.Users
                        select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.UserName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "Name_desc":
                    users = users.OrderByDescending(s => s.UserName);
                    break;
                case "Role":
                    users = users.OrderBy(s => s.RoleName);
                    break;
                case "Role_desc":
                    users = users.OrderByDescending(s => s.RoleName);
                    break;
                default:
                    users = users.OrderBy(s => s.UserName);
                    break;
            }
            foreach (var role in users)
            {
                var moselItem = new AdminUserViewModel(role);
                rolesList.Add(moselItem);
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(rolesList.ToPagedList(pageNumber, pageSize));
            
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.roles = _db.Roles.ToList();
            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
         public ActionResult Create(AdminUserViewModel model)
        {

            if (ModelState.IsValid)
            {
                var role = model.RoleName;

                var user = new ApplicationUser()
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FistName = model.FistName,
                    LastName = model.LastName,
                    Organization = model.Organization,
                    City = model.City,
                    Country = model.Country,
                    RoleName = model.RoleName
                };
              

                IdentityResult result = UserManager.Create(user, model.Password);
                _db.AddUserToRole(UserManager, user.Id, model.RoleName);
                _db.SaveChanges();

                return RedirectToAction("Index", "AdminUser");
            }
            else
                return View();
        }
        
        [Authorize(Roles = "Admin")]
        public ActionResult Details(string id = "")
        {
            ApplicationUser user = _db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id = "")
        {
            ApplicationUser user = _db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            EditUserViewModel model = new EditUserViewModel(user);
            ViewBag.roles = _db.Roles.ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
               ApplicationUser user =unityOfWork.UserRepository.GetByID(model.Id);
               user.LastName = model.LastName;
               user.FistName = model.FistName;
               user.Country = model.Country;
               user.City = model.City;
               user.Organization = model.Organization;
               if (user.RoleName != model.Role)
               {
                   _db.RemoveFromRole(UserManager, user.Id, model.Role);
                   user.RoleName = model.Role;
                   _db.AddUserToRole(UserManager, user.Id, model.Role);
               }
                unityOfWork.Save();
                
                return RedirectToAction("Index");
            }
            return View(model);
        }
        
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id = "")
        {
            ApplicationUser department = _db.Users.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser user = _db.Users.Find(id);
            _db.Users.Remove(user);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }  

    }
}
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TTP_Project.Models;
using TTP_Project.Models.entities;
using TTP_Project.Models.repository;

namespace TTP_Project.Controllers
{
    [Authorize(Roles = "AccountManager")]
    public class AccountManagerController : Controller
    {
        private ApplicationUserManager _userManager;

        private UnitOfWork unitOfWork = new UnitOfWork();

        public AccountManagerController() { }

        public AccountManagerController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
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

        public ActionResult Index()
        {
            IEnumerable<Finance> list = unitOfWork.FinancesRepository.Get();
            return View(list);
        }





    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TTP_Project.Models;
using TTP_Project.Models.constants;
using TTP_Project.Models.ViewModels;

namespace TTP_Project.Controllers
{
    public class DistributorController : Controller
    {

        private ApplicationDbContext _db = new ApplicationDbContext();

        public ActionResult Index()
        {
            
            if (User.IsInRole(RolesConst.DEVELOPER))
            {
                return RedirectToAction("Index", RolesConst.DEVELOPER);
            }
            if (User.IsInRole(RolesConst.ORDER_MANAGER))
            {
                return RedirectToAction("Index", RolesConst.ORDER_MANAGER);
            }
            if (User.IsInRole(RolesConst.PROJECT_MANAGER))
            {
                return RedirectToAction("Index", RolesConst.PROJECT_MANAGER);
            }
            
            if(User.IsInRole(RolesConst.CUSTOMER))
            {
                return RedirectToAction("Index", RolesConst.CUSTOMER);
            }
            if (User.IsInRole(RolesConst.ACCOUNT_MANAGER))
            {
                return RedirectToAction("Index", RolesConst.ACCOUNT_MANAGER);
            }
            return RedirectToAction("About", "Home");;
        }

       
	}
}
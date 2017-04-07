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
            
            if (User.IsInRole(RolesConst.PROGRAMER))
            {
                return RedirectToAction("Index", RolesConst.PROGRAMER);
            }
            if (User.IsInRole(RolesConst.OPERATOR))
            {
                return RedirectToAction("Index", RolesConst.OPERATOR);
            }
            if (User.IsInRole(RolesConst.RESOURSE_MANAGER))
            {
                return RedirectToAction("Index", RolesConst.RESOURSE_MANAGER+"s");
            }
            if (User.IsInRole(RolesConst.ORDER_OPERATOR))
            {
                return RedirectToAction("Index", RolesConst.ORDER_OPERATOR);
            }
            if(User.IsInRole(RolesConst.CUSTOMER))
            {
                return RedirectToAction("Index", RolesConst.CUSTOMER);
            }
            if (User.IsInRole(RolesConst.MANAGER))
            {
                return RedirectToAction("Index", RolesConst.MANAGER);
            }
            return RedirectToAction("About", "Home");;
        }

       
	}
}
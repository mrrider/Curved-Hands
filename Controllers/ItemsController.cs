using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using TTP_Project.Models;
using TTP_Project.Models.repository;
using TTP_Project.Models.entities;
using TTP_Project.Models.constants;

namespace TTP_Project.Controllers
{
    public class ItemsController : Controller
    {
        private UnitOfWork unityOfWork = new UnitOfWork();
        
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var items = from i in unityOfWork.ProductItemRepository.dbSet select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.Name.ToUpper().Contains(searchString.ToUpper())
                                       || s.Categorie.ToString().ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.Name);
                    break;
                case "Price":
                    items = items.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    items = items.OrderByDescending(s => s.Price);
                    break;
                default:  
                    items = items.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View( items.ToPagedList(pageNumber, pageSize));
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductItem item = await unityOfWork.ProductItemRepository.dbSet.FindAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        [Authorize(Roles = "SalesManager")]
        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SalesManager")]
        public async Task<ActionResult> Create(ProductItem item)
        {
            if (ModelState.IsValid)
            {
                unityOfWork.ProductItemRepository.context.ProductItems.Add(item);
                await unityOfWork.ProductItemRepository.context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(item);
        }

         [Authorize(Roles = "SalesManager")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductItem item = await unityOfWork.ProductItemRepository.context.ProductItems.FindAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SalesManager")]
        public async Task<ActionResult> Edit(ProductItem item)
        {
            if (ModelState.IsValid)
            {
                unityOfWork.ProductItemRepository.context.Entry(item).State = EntityState.Modified;
                await unityOfWork.ProductItemRepository.context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(item);
        }

         [Authorize(Roles = "SalesManager")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductItem item = await unityOfWork.ProductItemRepository.context.ProductItems.FindAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SalesManager")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ProductItem item = await unityOfWork.ProductItemRepository.context.ProductItems.FindAsync(id);
            unityOfWork.ProductItemRepository.context.ProductItems.Remove(item);
            await unityOfWork.ProductItemRepository.context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> RenderImage(int id)
        {
            ProductItem item = await unityOfWork.ProductItemRepository.context.ProductItems.FindAsync(id);

            byte[] photoBack = item.InternalImage;

            return File(photoBack, "image/png");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unityOfWork.ProductItemRepository.context.Dispose();
            }
            base.Dispose(disposing);
        }
       
    }
}

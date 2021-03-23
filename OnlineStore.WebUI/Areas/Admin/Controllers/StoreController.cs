using OnlineStore.Domain.Infrastructure;
using OnlineStore.Domain.Model;
using OnlineStore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineStore.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StoreController : Controller
    {
        private CategoryService _service = new CategoryService();

        // GET: Product
        public ActionResult Product()
        {
            List<Category> list = _service.GetAll();
            ViewBag.Categories = list;
            List<Category> alllist = new List<Category>(list);
            alllist.Insert(0, new Category { Id = 0, CategoryName = "Select All" });
            ViewBag.CategoryFilter = alllist;
            return View();
        }

        public ActionResult Category()
        {
            return View();
        }

        public ActionResult Order()
        {
            return View();
        }
    }
}
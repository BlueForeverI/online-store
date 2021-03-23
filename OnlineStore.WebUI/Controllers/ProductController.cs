using OnlineStore.Domain.Model;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using OnlineStore.Services.DTO;
using OnlineStore.Services;

namespace OnlineStore.WebUI.Controllers
{    
    public class ProductController : Controller
    {
        private ProductService _productService = new ProductService();
        private CategoryService _categoryService = new CategoryService();
        private OrderService _orderService = new OrderService();

        public ActionResult Console()
        {
            List<ProductDTO> list = GetProductsByCategory(1);
            ViewBag.Title = "Console";
            return View("List", list);
        }
        public ActionResult Accessory()
        {
            List<ProductDTO> list = GetProductsByCategory(2);
            ViewBag.Title = "Accessory";
            return View("List", list);
        }
        public ActionResult Game()
        {
            List<ProductDTO> list = GetProductsByCategory(3);
            ViewBag.Title = "Game";
            return View("List", list);
        }

        private List<ProductDTO> GetProductsByCategory(int categoryid)
        {
            var list = new List<ProductDTO>();
            if (System.Web.HttpContext.Current.Cache["ProductList" + categoryid] != null)
            {
                list = (List<ProductDTO>)System.Web.HttpContext.Current.Cache["ProductList" + categoryid];
            }
            else
            {
                list = _productService.GetProductDTOsByCategory(categoryid);
                System.Web.HttpContext.Current.Cache["ProductList" + categoryid] = list;
            }

            return list;
        }

        public ActionResult Search(string productname)
        {
            var list = new List<ProductDTO>();

            if (string.IsNullOrEmpty(productname))
            {
                list = _productService.GetProductDTOs();
            }
            else
            {
                list = _productService.GetProductDTOsByName(productname);
            }
            ViewBag.Title = "Search Result";
            return View("List", list);
        }

        public ActionResult Detail(int id)
        {
            return View(_productService.GetProductDTO(id));
        }

        [Authorize(Roles = "Admin, Advanced")]
        public ActionResult MyProducts()
        {
            List<Category> list = _categoryService.GetAll();
            ViewBag.Categories = list;

            List<Category> alllist = new List<Category>(list);
            alllist.Insert(0, new Category { Id = 0, CategoryName = "Select All" });
            ViewBag.CategoryFilter = alllist;
            return View();
        }

        [Authorize(Roles = "Admin, Advanced")]
        public ActionResult MyProductOrders()
        {
            List<ProductOrderDTO> list = new List<ProductOrderDTO>();
            try
            {
                string userid = User.Identity.GetUserId();
                list = _productService.GetUserProductOrdersDTOs(userid);             

                foreach (ProductOrderDTO product in list)
                {
                    product.Orders = _orderService.GetOrderDTOs(product.Id);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error Occurs:" + ex.Message;
            }

            return View(list);
        }
    }
}
﻿using OnlineStore.Domain.Infrastructure;
using OnlineStore.Domain.Model;
using OnlineStore.WebUI.Areas.Admin.Models.DTO;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineStore.WebUI.Controllers
{    
    public class ProductController : Controller
    {        
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
            List<ProductDTO> list = new List<ProductDTO>();
            if (System.Web.HttpContext.Current.Cache["ProductList" + categoryid] != null)
            {
                list = (List<ProductDTO>)System.Web.HttpContext.Current.Cache["ProductList" + categoryid];
            }
            else
            {
                using (OnlineStoreDBContext context = new OnlineStoreDBContext())
                {
                    var query = from product in context.Products
                                where product.CategoryId == categoryid
                                join category in context.Categories
                                  on product.CategoryId equals category.Id
                                select new ProductDTO { Id = product.Id, ProductName = product.ProductName, CategoryId = product.CategoryId, CategoryName = category.CategoryName, Price = product.Price, Image = product.Image, Condition = product.Condition, Discount = product.Discount, UserId = product.UserId };
                    list = query.ToList();
                    System.Web.HttpContext.Current.Cache["ProductList" + categoryid] = list;
                }
            }

            return list;
        }

        public ActionResult Search(string productname)
        {
            List<ProductDTO> list = new List<ProductDTO>();

            using (OnlineStoreDBContext context = new OnlineStoreDBContext())
            {
                if (String.IsNullOrEmpty(productname))
                {
                    var query = from product in context.Products
                                join category in context.Categories
                                  on product.CategoryId equals category.Id
                                select new ProductDTO { Id = product.Id, ProductName = product.ProductName, CategoryId = product.CategoryId, CategoryName = category.CategoryName, Price = product.Price, Image = product.Image, Condition = product.Condition, Discount = product.Discount, UserId = product.UserId };
                    list = query.ToList();
                }
                else
                {
                    var query = from product in context.Products
                                where product.ProductName.ToLower().Contains(productname.ToLower())
                                join category in context.Categories
                                  on product.CategoryId equals category.Id
                                select new ProductDTO { Id = product.Id, ProductName = product.ProductName, CategoryId = product.CategoryId, CategoryName = category.CategoryName, Price = product.Price, Image = product.Image, Condition = product.Condition, Discount = product.Discount, UserId = product.UserId };
                    list = query.ToList();
                }
            }
            ViewBag.Title = "Search Result";
            return View("List", list);
        }

        public ActionResult Detail(int id)
        {
            ProductDTO model = null;
            using (OnlineStoreDBContext context = new OnlineStoreDBContext())
            {
                var query = from product in context.Products
                            where product.Id == id
                            join category in context.Categories
                              on product.CategoryId equals category.Id
                            select new ProductDTO { Id = product.Id, ProductName = product.ProductName, CategoryId = product.CategoryId, CategoryName = category.CategoryName, Price = product.Price, Image = product.Image, Condition = product.Condition, Discount = product.Discount, UserId = product.UserId };
                model = query.FirstOrDefault();
            }
            return View(model);
        }

        [Authorize(Roles = "Admin, Advanced")]
        public ActionResult MyProducts()
        {
            List<Category> list = new List<Category>();
            using (OnlineStoreDBContext context = new OnlineStoreDBContext())
            {
                list = context.Categories.ToList();
            }

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
                String userid = User.Identity.GetUserId();
                using (OnlineStoreDBContext context = new OnlineStoreDBContext())
                {
                    var query = from product in context.Products
                                where product.UserId == userid
                                join category in context.Categories
                                  on product.CategoryId equals category.Id
                                select new ProductOrderDTO { Id = product.Id, ProductName = product.ProductName, CategoryId = product.CategoryId, CategoryName = category.CategoryName, Price = product.Price, Image = product.Image, Condition = product.Condition, Discount = product.Discount, UserId = product.UserId };
                    list = query.ToList();                    

                    foreach (ProductOrderDTO product in list)
                    {
                        var orders = from o in context.Orders
                                     join i in context.OrderItems
                                       on o.Id equals i.OrderId
                                     join u in context.Users
                                       on o.UserId equals u.Id
                                     where i.ProductId == product.Id
                                     orderby o.Id descending
                                     select new { o.Id, o.UserId, u.UserName, o.FullName, o.Address, o.City, o.State, o.Zip, o.ConfirmationNumber, o.DeliveryDate };
                        product.Orders = orders.Select(o => new OrderDTO { OrderId = o.Id, UserId = o.UserId, UserName = o.UserName, FullName = o.FullName, Address = o.Address, City = o.City, State = o.State, Zip = o.Zip, ConfirmationNumber = o.ConfirmationNumber, DeliveryDate = o.DeliveryDate }).ToList();

                    }
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
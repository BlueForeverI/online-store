using OnlineStore.Domain.Infrastructure;
using OnlineStore.WebUI.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineStore.Services;
using OnlineStore.ViewModels;

namespace OnlineStore.WebUI.Controllers
{
    [Authorize]
    public class MyOrderController : Controller
    {
        private OrderService _service = new OrderService();
        // GET: MyOrder
        public ActionResult Index()
        {
            List<OrderViewModel> list = new List<OrderViewModel>();
            try
            {
                string userid = User.Identity.GetUserId();
                var orders = _service.GetOrderDTOs();
                list = orders
                    .Select(o => new OrderViewModel { 
                        OrderId = o.OrderId, UserId = o.UserId, UserName = o.UserName, FullName = o.FullName, Address = o.Address, 
                        City = o.City, State = o.State, Zip = o.Zip, ConfirmationNumber = o.ConfirmationNumber, DeliveryDate = o.DeliveryDate 
                    }).ToList();


                foreach (OrderViewModel order in list)
                {
                    var orderitems = _service.GetOrderItemDTOs(order.OrderId);
                    order.Items = orderitems
                        .Select(o => new OrderItemViewModel { 
                            OrderItemId = o.OrderId, OrderId = o.OrderId, ProductId = o.ProductId, ProductName = o.ProductName, 
                            CategoryId = o.CategoryId, CategoryName = o.CategoryName, Price = o.Price, Image = o.Image, Condition = o.Condition, 
                            Discount = o.Discount, Quantity = o.Quantity
                        }).ToList();
                }
                Session["OrderCount"] = orders.Count();
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Грешка: " + ex.Message;
            }

            return View(list);
        }

        [HttpPost]
        public ActionResult CancelOrder(int id)
        {
            var order = _service.Get(id);
            if (order == null)
            {
                ViewBag.Message = string.Format("Поръчка [{0}] не е намеренa.", id);
            }
            else {
                _service.Delete(id);
                ViewBag.Message = string.Format("Поръчка [{0}] е изтрита!", id);
            }

            return RedirectToAction("Index");
        }
    }
}
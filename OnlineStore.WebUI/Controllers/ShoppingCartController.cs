using OnlineStore.Domain.Infrastructure;
using OnlineStore.Domain.Model;
using OnlineStore.WebUI.Helper;
using OnlineStore.WebUI.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OnlineStore.Services;
using OnlineStore.ViewModels;

namespace OnlineStore.WebUI.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private ProductService _productService = new ProductService();
        private OrderService _orderService = new OrderService();

        // GET: ShoppingCart
        public ActionResult Index()
        {
            ShoppingCart cart = (ShoppingCart)Session["ShoppingCart"];
            if (cart == null)
            {
                cart = new ShoppingCart();
                Session["ShoppingCart"] = cart;
            }
            return View(cart);
        }

        public ActionResult CreateOrUpdate(CartViewModel value)
        {
            ShoppingCart cart = (ShoppingCart)Session["ShoppingCart"];
            if (cart == null)
            {
                cart = new ShoppingCart();
                Session["ShoppingCart"] = cart;
            }

            Product product = _productService.Get(value.Id);
            if (product != null)
            {
                if (value.Quantity == 0)
                {
                    cart.AddItem(value.Id, product);
                }
                else
                {
                    cart.SetItemQuantity(value.Id, value.Quantity, product);
                }
            }

            Session["CartCount"] = cart.GetItems().Count();
            return View("Index", cart);
        }

        public ActionResult Checkout()
        {
            CheckoutViewModel checkout = new CheckoutViewModel();
            checkout.FullName = "Петър Петров";
            checkout.Address = "ж-к Люлин";
            checkout.City = "София";
            checkout.State = "София";
            checkout.Zip = "1000";
            ViewBag.States = State.List();
            return View(checkout);
        }

        public ActionResult PlaceOrder(CheckoutViewModel value)
        {          
            ShoppingCart cart = (ShoppingCart)Session["ShoppingCart"];
            if (cart == null)
            {
                ViewBag.Message = "Количката ви е празна!";
                return View("Index", "ShoppingCart");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Моля въведете валиден адрес за доставка!";
                return View("Checkout", "ShoppingCart");
            }

            Session["Checkout"] = value;

            return View("Index", "ShoppingCart");
        }

        public ActionResult ProcessCreditResponse(String TransId, String TransAmount, String StatusCode, String AppHash)
        {
            string AppId = ConfigurationHelper.GetAppId2();
            string SharedKey = ConfigurationHelper.GetSharedKey2();

            if (CreditAuthorizationClient.VerifyServerResponseHash(AppHash, SharedKey, AppId, TransId, TransAmount, StatusCode))
            {
                switch (StatusCode)
                {
                    case ("A"): ViewBag.TransactionStatus = "Поръчката ви е създадена успешно!"; break;
                    case ("D"): ViewBag.TransactionStatus = "Транзакцията е отказана!"; break;
                    case ("C"): ViewBag.TransactionStatus = "Транзакцията е прекратена!"; break;
                }
            }
            else
            {
                ViewBag.TransactionStatus = "Грешка при потвърждение на плащането.";
            }

            OrderViewModel model = new OrderViewModel();

            if (StatusCode.Equals("A"))
            {
                ShoppingCart cart = (ShoppingCart)Session["ShoppingCart"];
                CheckoutViewModel value = (CheckoutViewModel)Session["Checkout"];
                if (value != null)
                {                    
                    try
                    {
                        Order newOrder = new Order();
                        newOrder.FullName = value.FullName;
                        newOrder.Address = value.Address;
                        newOrder.City = value.City;
                        newOrder.State = value.State;
                        newOrder.Zip = value.Zip;
                        newOrder.DeliveryDate = DateTime.Now.AddDays(14);
                        newOrder.ConfirmationNumber = DateTime.Now.ToString("yyyyMMddHHmmss");
                        newOrder.UserId = User.Identity.GetUserId();
                        newOrder = _orderService.Add(newOrder);

                        cart.GetItems().ForEach(c => 
                            _orderService.AddOrderItem(
                                new OrderItem { OrderId = newOrder.Id, ProductId = c.GetItemId(), 
                                    Quantity = c.Quantity 
                                }));
                        System.Web.HttpContext.Current.Cache.Remove("OrderList");
                        Session["ShoppingCart"] = null;
                        Session["CartCount"] = 0;
                        Session["OrderCount"] = (int)Session["OrderCount"] + 1;

                        model = _orderService.GetOrderViewModel(newOrder.Id);
                        model.Items = _orderService.GetOrderItemViewModels(newOrder.Id);
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "Error Occurs:" + ex.Message;
                    }
                }
            }

            return View("PlaceOrder", model);
        }
    }
}
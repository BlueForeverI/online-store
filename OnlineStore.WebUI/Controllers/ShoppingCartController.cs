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

namespace OnlineStore.WebUI.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
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
            using (OnlineStoreDBContext context = new OnlineStoreDBContext())
            {
                Product product = context.Products.Find(value.Id);
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
            }

            Session["CartCount"] = cart.GetItems().Count();
            return View("Index", cart);
        }

        public ActionResult Checkout()
        {
            CheckoutViewModel checkout = new CheckoutViewModel();
            checkout.FullName = "Peter Petrov";
            checkout.Address = "Serdika str.";
            checkout.City = "Sofia";
            checkout.State = "Sofia";
            checkout.Zip = "1000";
            ViewBag.States = State.List();
            return View(checkout);
        }

        public ActionResult PlaceOrder(CheckoutViewModel value)
        {          
            ShoppingCart cart = (ShoppingCart)Session["ShoppingCart"];
            if (cart == null)
            {
                ViewBag.Message = "Your cart is empty!";
                return View("Index", "ShoppingCart");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Please provide valid shipping address!";
                return View("Checkout", "ShoppingCart");
            }

            Session["Checkout"] = value;

            //Assign the values for the properties we need to pass to the service
            String AppId = ConfigurationHelper.GetAppId2();
            String SharedKey = ConfigurationHelper.GetSharedKey2();
            String AppTransId = "20";
            String AppTransAmount = cart.GetTotalValue().ToString();

            // Hash the values so the server can verify the values are original
            String hash = HttpUtility.UrlEncode(CreditAuthorizationClient.GenerateClientRequestHash(SharedKey, AppId, AppTransId, AppTransAmount));

            //Create the URL and  concatenate  the Query String values
            String url = "http://ectweb2.cs.depaul.edu/ECTCreditGateway/Authorize.aspx";
            url = url + "?AppId=" + AppId;
            url = url + "&TransId=" + AppTransId;
            url = url + "&AppTransAmount=" + AppTransAmount;
            url = url + "&AppHash=" + hash;

            return Redirect(url);
        }

        public ActionResult ProcessCreditResponse(String TransId, String TransAmount, String StatusCode, String AppHash)
        {
            String AppId = ConfigurationHelper.GetAppId2();
            String SharedKey = ConfigurationHelper.GetSharedKey2();

            if (CreditAuthorizationClient.VerifyServerResponseHash(AppHash, SharedKey, AppId, TransId, TransAmount, StatusCode))
            {
                switch (StatusCode)
                {
                    case ("A"): ViewBag.TransactionStatus = "Transaction Approved! Your order has been created!"; break;
                    case ("D"): ViewBag.TransactionStatus = "Transaction Denied!"; break;
                    case ("C"): ViewBag.TransactionStatus = "Transaction Cancelled!"; break;
                }
            }
            else
            {
                ViewBag.TransactionStatus = "Hash Verification failed... something went wrong.";
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
                        using (OnlineStoreDBContext context = new OnlineStoreDBContext())
                        {
                            Order newOrder = context.Orders.Create();
                            newOrder.FullName = value.FullName;
                            newOrder.Address = value.Address;
                            newOrder.City = value.City;
                            newOrder.State = value.State;
                            newOrder.Zip = value.Zip;
                            newOrder.DeliveryDate = DateTime.Now.AddDays(14);
                            newOrder.ConfirmationNumber = DateTime.Now.ToString("yyyyMMddHHmmss");
                            newOrder.UserId = User.Identity.GetUserId();
                            context.Orders.Add(newOrder);
                            cart.GetItems().ForEach(c => context.OrderItems.Add(new OrderItem { OrderId = newOrder.Id, ProductId = c.GetItemId(), Quantity = c.Quantity }));
                            context.SaveChanges();
                            System.Web.HttpContext.Current.Cache.Remove("OrderList");
                            Session["ShoppingCart"] = null;
                            Session["CartCount"] = 0;
                            Session["OrderCount"] = (int)Session["OrderCount"] + 1;

                            var order = from o in context.Orders
                                        join u in context.Users
                                          on o.UserId equals u.Id
                                        where o.Id == newOrder.Id
                                        select new { o.Id, o.UserId, u.UserName, o.FullName, o.Address, o.City, o.State, o.Zip, o.ConfirmationNumber, o.DeliveryDate };
                            var ord = order.FirstOrDefault();
                            model = new OrderViewModel { OrderId = ord.Id, UserId = ord.UserId, UserName = ord.UserName, FullName = ord.FullName, Address = ord.Address, City = ord.City, State = ord.State, Zip = ord.Zip, ConfirmationNumber = ord.ConfirmationNumber, DeliveryDate = ord.DeliveryDate };

                            var orderitems = from i in context.OrderItems
                                             join p in context.Products
                                               on i.ProductId equals p.Id
                                             join c in context.Categories
                                               on p.CategoryId equals c.Id
                                             where i.OrderId == newOrder.Id
                                             select new { i.Id, i.OrderId, i.ProductId, p.ProductName, p.CategoryId, c.CategoryName, p.Price, p.Image, p.Condition, p.Discount, i.Quantity };
                            model.Items = orderitems.Select(o => new OrderItemViewModel { OrderItemId = o.Id, OrderId = o.OrderId, ProductId = o.ProductId, ProductName = o.ProductName, CategoryId = o.CategoryId, CategoryName = o.CategoryName, Price = o.Price, Image = o.Image, Condition = o.Condition, Discount = o.Discount, Quantity = o.Quantity }).ToList();
                        }
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
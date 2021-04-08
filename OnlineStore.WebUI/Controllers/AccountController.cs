using OnlineStore.Domain.Identity;
using OnlineStore.WebUI.Helper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using OnlineStore.Services.DTO;
using OnlineStore.Services;
using OnlineStore.ViewModels;

namespace OnlineStore.WebUI.Controllers
{
    public class AccountController : BaseController
    {
        private UserService _userService = new UserService();
        private OrderService _orderService = new OrderService();

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = UserManager.FindByEmail(model.Email);
                if (user != null)
                {
                    ModelState.AddModelError("", "Потребител с този email адрес вече съществува! Пробвайте с друг email.");
                    return View(model);
                }
                user = UserManager.FindByName(model.UserName);
                if (user != null)
                {
                    ModelState.AddModelError("", "Потребител с такова име вече съществува! ПРобвайте с друго име.");
                    return View(model);
                }

                _userService.Register(model.Email, model.Password, model.UserName, model.Membership);
                Session["Register"] = model;

                return View("RegistrationSuccessful");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> ProcessCreditResponse(string TransId, string TransAmount, string StatusCode, string AppHash)
        {
            string AppId = ConfigurationHelper.GetAppId();
            string SharedKey = ConfigurationHelper.GetSharedKey();

            if (CreditAuthorizationClient.VerifyServerResponseHash(AppHash, SharedKey, AppId, TransId, TransAmount, StatusCode))
            {
                switch (StatusCode)
                {
                    case ("A"): ViewBag.TransactionStatus = "Транзакцията е одобрена!"; break;
                    case ("D"): ViewBag.TransactionStatus = "Транзакцията е отказана!"; break;
                    case ("C"): ViewBag.TransactionStatus = "Транзакцията е прекратена!"; break;
                }
            }
            else
            {
                ViewBag.TransactionStatus = "Грешка при потвърждението.";
            }


            if (StatusCode.Equals("A"))
            {
                RegisterViewModel model = (RegisterViewModel)Session["Register"];
                if (model != null)
                {
                    var user = new AppUser { Email = model.Email, UserName = model.UserName, Membership = model.Membership };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        var newUser = UserManager.FindByEmail(model.Email);
                        var identity = await UserManager.CreateIdentityAsync(newUser, DefaultAuthenticationTypes.ApplicationCookie);
                        AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);

                        System.Web.HttpContext.Current.Cache.Remove("UserList");
                        Session["Register"] = null;

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            return View();
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    {
                        var user = UserManager.FindByEmail(model.Email);
                        var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                        AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);
                        GetOrderCount(user.Id);
                        if (!String.IsNullOrEmpty(returnUrl))
                            return RedirectToLocal(returnUrl);
                        else
                            return RedirectToAction("Index", "Home");
                    }
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Грешка при влизането! Проверете потребителското име и паролата");
                    return View(model);
            }
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session["OrderCount"] = 0;
            Session["CartCount"] = 0;
            return RedirectToAction("Index", "Home");
        }

        private void GetOrderCount(string id)
        {
            int count = _orderService.CountUserOrders(id);
            Session["OrderCount"] = count;
            Session["CartCount"] = 0;
        }

        [Authorize]
        public ActionResult MemberProfile()
        {
            AppUser u = _userService.Get(User.Identity.GetUserId());
            UserDTO user = new UserDTO { 
                Id = u.Id, Email = u.Email, UserName = u.UserName, Membership = u.Membership 
            };
            return View(user);
        }

        //
        // GET: /Manage/ChangePassword
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", "Home");
            }
            AddErrors(result);
            return View(model);
        }

    }
}
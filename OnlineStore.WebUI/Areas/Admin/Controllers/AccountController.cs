using OnlineStore.WebUI.Areas.Admin.Models;
using OnlineStore.Domain.Infrastructure;
using OnlineStore.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineStore.Domain.Identity;
using OnlineStore.WebUI.Controllers;
using OnlineStore.Services.DTO;

namespace OnlineStore.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccountController : BaseController
    {
        // GET: User
        public ActionResult AppUser()
        {
            var roles = RoleManager.Roles.ToList();
            List<RoleDTO> list = roles.Select(r => new RoleDTO { Id = r.Id, Name = r.Name, Description = r.Description }).ToList();
            ViewBag.Roles = list;
            return View();
        }
        public ActionResult AppRole()
        {
            return View();
        }
    }
}
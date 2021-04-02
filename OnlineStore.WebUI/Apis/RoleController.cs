using OnlineStore.Domain.Identity;
using OnlineStore.WebUI.Areas.Admin.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using OnlineStore.Services.DTO;

namespace OnlineStore.WebUI.Apis
{
    [Authorize(Roles = "Admin")]
    public class RoleController : BaseApiController
    {
        // GET api/<controller>
        public List<RoleDTO> Get()
        {
            if (HttpContext.Current.Cache["RoleList"] != null)
            {
                return (List<RoleDTO>)HttpContext.Current.Cache["RoleList"];
            }
            else
            {
                List<RoleDTO> roles = RoleManager
                    .Roles
                    .Select(r => new RoleDTO { 
                        Id = r.Id, Name = r.Name, Description = r.Description 
                    }).ToList();
                HttpContext.Current.Cache["RoleList"] = roles;
                return roles;
            }
        }

        // GET api/<controller>/5
        public RoleDTO Get(string id)
        {
            if (HttpContext.Current.Cache["Role" + id] != null)
            {
                return (RoleDTO)HttpContext.Current.Cache["Role" + id];
            }
            else
            {
                AppRole r = RoleManager.FindById(id);
                RoleDTO role = new RoleDTO { 
                    Id = r.Id, Name = r.Name, Description = r.Description 
                };
                HttpContext.Current.Cache["Role" + id] = role;
                return role;
            }            
        }

        // GET: api/Category/GetCount/
        [Route("api/Role/GetCount")]
        public int GetCount()
        {
            if (HttpContext.Current.Cache["RoleList"] != null)
            {
                List<RoleDTO> list = (List<RoleDTO>)HttpContext.Current.Cache["RoleList"];
                return list.Count();
            }
            else
            {
                List<RoleDTO> roles = RoleManager
                    .Roles
                    .Select(r => new RoleDTO { 
                        Id = r.Id, Name = r.Name, Description = r.Description 
                    }).ToList();
                HttpContext.Current.Cache["RoleList"] = roles;
                return roles.Count();
            }
        }

        [Route("api/Role/Create")]
        public HttpResponseMessage Create([FromBody]RoleViewModel value)
        {
            if (ModelState.IsValid)
            {
                AppRole existRole = RoleManager.FindByName(value.Name);
                if (existRole != null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        "Роля [" + value.Name + "] вече съществува, изберете друго име!");
                }

                AppRole role = new AppRole();
                role.Name = value.Name;
                role.Description = value.Description;
                IdentityResult result = RoleManager.Create(role);
                if (result.Succeeded)
                {
                    HttpContext.Current.Cache.Remove("RoleList");
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, GetErrorMessage(result));
                }                
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Грешни данни");
            }
        }
        public HttpResponseMessage Post([FromBody]RoleViewModel value)
        {
            if (ModelState.IsValid)
            {
                AppRole role = RoleManager.FindById(value.Id);
                if (role == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                        "Роля [" + value.Id + "] не съществува!");
                }
                role.Name = value.Name;
                role.Description = value.Description;
                IdentityResult result = RoleManager.Update(role);
                if (result.Succeeded)
                {
                    HttpContext.Current.Cache.Remove("RoleList");
                    HttpContext.Current.Cache.Remove("Role" + role.Id);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        GetErrorMessage(result));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Грешни данни");
            }            
        }
        
        // DELETE api/<controller>/5
        public HttpResponseMessage Delete(string id)
        {
            AppRole role = RoleManager.FindById(id);
            if (role == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound,
                    "Роля ["+id+"] не е намерена.");
            }
            else if (role.Users.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    "Има потребители за роля [" + role.Name + "], изтрийте тях първо!");
            }
            else
            {
                IdentityResult result = RoleManager.Delete(role);
                if (result.Succeeded)
                {
                    HttpContext.Current.Cache.Remove("RoleList");
                    HttpContext.Current.Cache.Remove("Role" + id);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        GetErrorMessage(result));
                }
            }            
        }
    }
}

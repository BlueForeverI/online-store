using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Http;
using OnlineStore.Domain.Infrastructure;
using OnlineStore.Domain.Model;
using System.Net.Http;
using System.Net;
using OnlineStore.WebUI.Areas.Admin.Models.DTO;
using OnlineStore.WebUI.Areas.Admin.Models;
using System.Linq.Expressions;
using OnlineStore.Services;

namespace OnlineStore.WebUI.Apis
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : ApiController
    {
        private CategoryService _service = new CategoryService(); 

        // GET api/<controller>
        public List<CategoryDTO> Get()
        {
            if (HttpContext.Current.Cache["CategoryList"] != null)
                return (List<CategoryDTO>)HttpContext.Current.Cache["CategoryList"];
            List<CategoryDTO> categories = _service.GetAll().AsQueryable().Select(ToDto()).ToList();
            HttpContext.Current.Cache["CategoryList"] = categories;
            return categories;
        }

        private static Expression<Func<Category, CategoryDTO>> ToDto()
        {
            return c => new CategoryDTO { CategoryId = c.Id, CategoryName = c.CategoryName };
        }

        // GET api/<controller>/5
        public CategoryDTO Get(int id)
        {
            if (HttpContext.Current.Cache["Category" + id] != null)
                return (CategoryDTO)HttpContext.Current.Cache["Category" + id];
            var c = _service.Get(id);
            CategoryDTO category = new CategoryDTO { CategoryId = c.Id, CategoryName = c.CategoryName };
            HttpContext.Current.Cache["Category" + id] = category;
            return category;
        }

        // GET: api/Category/GetCount/
        [Route("api/Category/GetCount")]
        public int GetCount()
        {
            if (HttpContext.Current.Cache["CategoryList"] != null)
            {
                List<CategoryDTO> list = (List<CategoryDTO>)HttpContext.Current.Cache["CategoryList"];
                return list.Count();
            }
            else
            {
                List<CategoryDTO> categories = _service.GetAll().AsQueryable().Select(ToDto()).ToList();
                HttpContext.Current.Cache["CategoryList"] = categories;
                return categories.Count();
            }
        }

        // POST api/<controller>
        [Route("api/Category/Create")]
        public HttpResponseMessage Create([FromBody]CategoryViewModel value)
        {
            if (value==null || String.IsNullOrEmpty(value.CategoryName))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Category Name can't be empty!");
            }            

            bool exist = _service.Exists(c => c.CategoryName.Equals(value.CategoryName, StringComparison.OrdinalIgnoreCase));
            if (exist)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Category ["+ value.CategoryName + "] already exists, please try another name!");
            }

            Category category = new Category { CategoryName = value.CategoryName };
            _service.Add(category);
            HttpContext.Current.Cache.Remove("CategoryList");
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        //PUT REMOVED
        // Ajax.htmlForm does not support put and delete, only supports get and post.
        public HttpResponseMessage Post([FromBody]CategoryViewModel value)
        {
            if (value == null || String.IsNullOrEmpty(value.CategoryName))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Category Name can't be empty!");
            }

            bool exist = _service.Exists(c => c.Id == value.CategoryId);
            if (!exist)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Category ["+value.CategoryId+"] does not exist!");
            }
                
            exist = _service.GetAll()
                .Where(c => c.Id != value.CategoryId)
                .Any(c => c.CategoryName.Equals(value.CategoryName, StringComparison.OrdinalIgnoreCase));
            if (exist)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    "Category [" + value.CategoryName + "] already exists, please try another name!");
            }

            var category = _service.Get(value.CategoryId);
            category.CategoryName = value.CategoryName;
            _service.Update(category);

            HttpContext.Current.Cache.Remove("CategoryList");
            HttpContext.Current.Cache.Remove("Category" + value.CategoryId);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // DELETE api/<controller>/5
        public HttpResponseMessage Delete(int id)
        {
            var productService = new ProductService();
            bool exist = productService.Exists(p => p.CategoryId == id);
            if (exist)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    "Theis product belongs to Category [" + id + "], delete them first!");
            }

            _service.Delete(id);
            HttpContext.Current.Cache.Remove("CategoryList");
            HttpContext.Current.Cache.Remove("Category" + id);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using OnlineStore.Domain.Model;
using System.Net.Http;
using System.Net;
using Microsoft.AspNet.Identity;
using OnlineStore.WebUI.Areas.Admin.Models;
using OnlineStore.Services.DTO;
using OnlineStore.Services;

namespace OnlineStore.WebUI.Apis
{
    public class ProductController : BaseApiController
    {
        private ProductService _service = new ProductService();

        // GET api/<controller>
        public List<ProductDTO> Get([FromUri] CategoryViewModel value)
        {
            if (value.CategoryId == 0)
            {
                if (HttpContext.Current.Cache["ProductList"] != null)
                    return (List<ProductDTO>)HttpContext.Current.Cache["ProductList"];
            }
            else
            {
                if (HttpContext.Current.Cache["ProductList" + value.CategoryId] != null)
                    return (List<ProductDTO>)HttpContext.Current.Cache["ProductList" + value.CategoryId];
            }
                
            if (value.CategoryId == 0)
            {
                List<ProductDTO> products = _service.GetProductDTOs();
                HttpContext.Current.Cache["ProductList"] = products;
                return products;
            }
            else
            {
                List<ProductDTO> products = _service.GetProductDTOsByCategory(value.CategoryId);
                HttpContext.Current.Cache["ProductList" + value.CategoryId] = products;
                return products;                 
            }
        }

        [Authorize(Roles = "Admin, Advanced")]
        [Route("api/Product/GetByUserId")]
        public List<ProductDTO> GetByUserId([FromUri] CategoryViewModel value)
        {
            var userId = User.Identity.GetUserId();
            if (value.CategoryId == 0)
            {
                return _service.GetUserProductDTOs(userId);
            }
            else
            {
                return _service.GetUserProductDTOsByCategory(userId, value.CategoryId);
            }
        }

        // GET api/<controller>/5
        public ProductDTO Get(int id)
        {
            if (HttpContext.Current.Cache["Product" + id] != null)
                return (ProductDTO)HttpContext.Current.Cache["Product" + id];

            var product = _service.Get(id);
            if (product == null)
            {
                return null;
            }
            else
            {
                ProductDTO productDTO = new ProductDTO { Id = product.Id, ProductName = product.ProductName, CategoryId = product.CategoryId, Price = product.Price, Image = product.Image, Condition = product.Condition, Discount = product.Discount, UserId = product.UserId };
                HttpContext.Current.Cache["Product" + id] = productDTO;
                return productDTO;
            }

        }

        // GET: api/Product/GetCount/
        [Route("api/Product/GetCount")]
        public int GetCount()
        {
            if (HttpContext.Current.Cache["ProductList"] != null)
            {
                List<ProductDTO> list = (List<ProductDTO>)HttpContext.Current.Cache["ProductList"];
                return list.Count();
            }
            else
            {
                List<ProductDTO> products = _service.GetProductDTOs();
                HttpContext.Current.Cache["ProductList"] = products;
                return products.Count();
            }
        }

        [Route("api/Product/GetAutoComplete")]
        public List<String> GetAutoComplete([FromUri] string name)
        {
            return _service.GetMatchingProductNames(name);
        }

        [Authorize(Roles = "Admin, Advanced")]
        [Route("api/Product/Create")]
        public HttpResponseMessage Create([FromBody]ProductViewModel value)
        {
            if (ModelState.IsValid)
            {
                if (value.Discount < 0 || value.Discount > 100)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        "Отстъпката трябва да е между 0 и 100.");
                }

                bool exist = _service.Exists(c => c.ProductName.Equals(value.ProductName, StringComparison.OrdinalIgnoreCase));
                if (exist)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        "Продукт с име [" + value.ProductName + "] вече съществува, изберете друго име!");
                }

                Product newProduct = new Product();
                newProduct.ProductName = value.ProductName;
                newProduct.CategoryId = value.CategoryId;
                newProduct.Price = value.Price;
                newProduct.Image = value.Image;
                newProduct.Condition = value.Condition;
                newProduct.Discount = value.Discount;
                newProduct.UserId = User.Identity.GetUserId();
                _service.Add(newProduct);

                HttpContext.Current.Cache.Remove("ProductList");
                HttpContext.Current.Cache.Remove("ProductList" + newProduct.CategoryId);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [Authorize(Roles = "Admin, Advanced")]
        public HttpResponseMessage Post([FromBody]ProductViewModel value)
        {
            if (ModelState.IsValid)
            {
                if (value == null || String.IsNullOrEmpty(value.ProductName))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Името на продукта не може да е празно!");
                }

                if (value.Discount < 0 || value.Discount > 100)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        "Отстъпката трябва да е между 0 и 100.");
                }

                bool exists = _service.Exists(c => c.Id == value.Id);
                if (!exists)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                        "Продукт [" + value.Id + "] не съществува!");
                }

                exists = _service.Exists(c => c.Id != value.Id &&
                    c.ProductName.Equals(value.ProductName,
                        StringComparison.OrdinalIgnoreCase));
                if (exists)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        "Продукт с име [" + value.ProductName + "] вече съществува, изберете друго име!");
                }
                var product = _service.Get(value.Id);
                if (product == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Няма такъв продукт!");
                }

                bool isAdvanced = HttpContext.Current.User.IsInRole("Advanced");
                if (isAdvanced && product.UserId != HttpContext.Current.User.Identity.GetUserId())
                {
                    return Request.CreateResponse(HttpStatusCode.Forbidden,
                        "Нямате права да редактирате този продукт!");
                }

                HttpContext.Current.Cache.Remove("ProductList" + product.CategoryId);
                product.ProductName = value.ProductName;
                product.CategoryId = value.CategoryId;
                product.Price = value.Price;
                product.Image = value.Image;
                product.Condition = value.Condition;
                product.Discount = value.Discount;
                _service.Update(product);
                HttpContext.Current.Cache.Remove("ProductList");
                HttpContext.Current.Cache.Remove("ProductList" + value.CategoryId);
                HttpContext.Current.Cache.Remove("Product" + product.Id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/<controller>/5
        [Authorize(Roles = "Admin, Advanced")]
        public HttpResponseMessage Delete(int id)
        {
            var product = _service.Get(id);
            if (product == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Няма такъв продукт!");
            }
            bool isAdvanced = HttpContext.Current.User.IsInRole("Advanced");
            if (isAdvanced && product.UserId != HttpContext.Current.User.Identity.GetUserId())
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden,
                    "Нямате права да изтриете този продукт!");
            }
            _service.Delete(id);

            HttpContext.Current.Cache.Remove("ProductList");
            HttpContext.Current.Cache.Remove("ProductList" + product.CategoryId);
            HttpContext.Current.Cache.Remove("Product" + id);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
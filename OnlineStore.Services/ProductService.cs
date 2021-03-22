using OnlineStore.Domain.Infrastructure;
using OnlineStore.Domain.Model;
using OnlineStore.Services.DTO;
using System.Collections.Generic;
using System.Linq;

namespace OnlineStore.Services
{
    public class ProductService : BaseService<Product>
    {
        public List<ProductDTO> GetProductDTOs()
        {
            using (OnlineStoreDBContext context = new OnlineStoreDBContext())
            {
                var query = from product in context.Products
                            join category in context.Categories
                              on product.CategoryId equals category.Id
                            select new ProductDTO { Id = product.Id, ProductName = product.ProductName, CategoryId = product.CategoryId, CategoryName = category.CategoryName, Price = product.Price, Image = product.Image, Condition = product.Condition, Discount = product.Discount, UserId = product.UserId };

                return query.ToList();
            }
        }

        public List<ProductDTO> GetProductDTOsByCategory(int categoryId)
        {
            using (OnlineStoreDBContext context = new OnlineStoreDBContext())
            {
                var query = from product in context.Products
                            where product.CategoryId == categoryId
                            join category in context.Categories
                              on product.CategoryId equals category.Id
                            select new ProductDTO { Id = product.Id, ProductName = product.ProductName, CategoryId = product.CategoryId, CategoryName = category.CategoryName, Price = product.Price, Image = product.Image, Condition = product.Condition, Discount = product.Discount, UserId = product.UserId };
                return query.ToList();
            }
        }

        public List<ProductDTO> GetUserProductDTOs(string userId)
        {
            using (OnlineStoreDBContext context = new OnlineStoreDBContext())
            {
                var query = from product in context.Products
                            where product.UserId == userId
                            join category in context.Categories
                              on product.CategoryId equals category.Id
                            select new ProductDTO { Id = product.Id, ProductName = product.ProductName, CategoryId = product.CategoryId, CategoryName = category.CategoryName, Price = product.Price, Image = product.Image, Condition = product.Condition, Discount = product.Discount, UserId = product.UserId };

                return query.ToList();
            }
        }

        public List<ProductDTO> GetUserProductDTOsByCategory(string userId, int categoryId)
        {
            using (OnlineStoreDBContext context = new OnlineStoreDBContext())
            {
                var query = from product in context.Products
                            where product.CategoryId == categoryId
                               && product.UserId == userId
                            join category in context.Categories
                              on product.CategoryId equals category.Id
                            select new ProductDTO { Id = product.Id, ProductName = product.ProductName, CategoryId = product.CategoryId, CategoryName = category.CategoryName, Price = product.Price, Image = product.Image, Condition = product.Condition, Discount = product.Discount, UserId = product.UserId };
                return query.ToList();
            }
        }

        public List<string> GetMatchingProductNames(string name)
        {
            using (OnlineStoreDBContext context = new OnlineStoreDBContext())
            {
                return (from product in context.Products
                            where product.ProductName.ToLower().Contains(name.ToLower())
                            select product.ProductName).ToList();
            }
        }
    }
}

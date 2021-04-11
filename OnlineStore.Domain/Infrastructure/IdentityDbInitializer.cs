using OnlineStore.Domain.Identity;
using OnlineStore.Domain.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.Domain.Identity;

namespace OnlineStore.Domain.Infrastructure
{
    public class IdentityDbInitializer : DropCreateDatabaseIfModelChanges<OnlineStoreDBContext>
    {
        protected override void Seed(OnlineStoreDBContext context)
        {
            PerformInitialSetup(context);
            base.Seed(context);
        }

        public void PerformInitialSetup(OnlineStoreDBContext context)
        {
            GetRoles().ForEach(c => context.Roles.Add(c));
            GetCategories().ForEach(c => context.Categories.Add(c));
            GetProducts().ForEach(c => context.Products.Add(c));
            context.SaveChanges();
            var hasher = new CustomHasher();
            var user = new AppUser { UserName = "admin", Email = "admin@gamestore.com", PasswordHash = hasher.HashPassword("admin"), Membership = "Admin", EmailConfirmed = true };
            var role = context.Roles.Where(r => r.Name == "Admin").First();
            user.Roles.Add(new IdentityUserRole { RoleId = role.Id, UserId = user.Id });
            context.Users.Add(user);
            user = new AppUser { UserName = "regular", Email = "regular@gamestore.com", PasswordHash = hasher.HashPassword("regular"), Membership = "Regular", EmailConfirmed = true };
            role = context.Roles.Where(r => r.Name == "Regular").First();
            user.Roles.Add(new IdentityUserRole { RoleId = role.Id, UserId = user.Id });
            context.Users.Add(user);
            user = new AppUser { UserName = "advanced", Email = "advanced@gamestore.com", PasswordHash = hasher.HashPassword("advanced"), Membership = "Advanced", EmailConfirmed = true };
            role = context.Roles.Where(r => r.Name == "Advanced").First();
            user.Roles.Add(new IdentityUserRole { RoleId = role.Id, UserId = user.Id });
            context.Users.Add(user);
            context.SaveChanges();
        }

        private static List<AppRole> GetRoles()
        {
            var roles = new List<AppRole> {
               new AppRole {Name="Admin", Description="Admin"},
               new AppRole {Name="Regular", Description="Regular"},
               new AppRole {Name="Advanced", Description="Advanced"}
            };

            return roles;
        }

        private static List<Category> GetCategories()
        {
            var categories = new List<Category> {
               new Category {Id=1, CategoryName="Конзоли"},
               new Category {Id=2, CategoryName="Аксесоари"},
               new Category {Id=3, CategoryName="Игри"}
            };

            return categories;
        }

        private static List<Product> GetProducts()
        {
            var imageUrl = "http://pm1.narvii.com/5994/c99d7523cb8063c40632b259e3d222162961eccf_00.jpg";
            var products = new List<Product> {
               new Product {Id=1, ProductName="XBox One", CategoryId = 1, Price = 399.00, Image=imageUrl, Condition="Нов", Discount=10, UserId="Admin"},
               new Product {Id=2, ProductName="XBox 360", CategoryId = 1, Price = 299.00, Image=imageUrl, Condition="Нов", Discount=10, UserId="Admin"},
               new Product {Id=3, ProductName="PS3", CategoryId = 1, Price = 219.00, Image=imageUrl, Condition="Нов", Discount=10, UserId="Admin"},
               new Product {Id=4, ProductName="PS4", CategoryId = 1, Price = 349.00, Image=imageUrl, Condition="Нов", Discount=10, UserId="Admin"},
               new Product {Id=5, ProductName="Wii", CategoryId = 1, Price = 269.00, Image=imageUrl, Condition="Нов", Discount=10, UserId="Admin"},
               new Product {Id=6, ProductName="WiiU", CategoryId = 1, Price = 299.99, Image=imageUrl, Condition="Нов", Discount=10, UserId="Admin"},
               new Product {Id=7, ProductName="Xbox Controller", CategoryId = 2, Price = 40.99, Image=imageUrl, Condition="Нов", Discount=10, UserId="Admin"},
               new Product {Id=8, ProductName="Turtle Beach Headset", CategoryId = 2, Price = 50.00, Image=imageUrl, Condition="Нов", Discount=10, UserId="Admin"},
               new Product {Id=9, ProductName="Speeding Wheel", CategoryId = 2, Price = 35.99, Image=imageUrl, Condition="Нов", Discount=10, UserId="Admin"},
               new Product {Id=10, ProductName="Wireless Adapter", CategoryId = 2, Price = 40.99, Image=imageUrl, Condition="Нов", Discount=10, UserId="Admin"},
               new Product {Id=11, ProductName="Wireless Controller", CategoryId = 2, Price = 19.99, Image=imageUrl, Condition="Нов", Discount=10, UserId="Admin"},
               new Product {Id=12, ProductName="Disc Remote Control", CategoryId = 2, Price = 23.99, Image=imageUrl, Condition="Нов", Discount=10, UserId="Admin"},
               new Product {Id=13, ProductName="Chartboost - Black", CategoryId = 2, Price = 18.99, Image=imageUrl, Condition="Нов", Discount=10, UserId="Admin"},
               new Product {Id=14, ProductName="Dual Controller Charger", CategoryId = 2, Price = 25.99, Image=imageUrl, Condition="Нов", Discount=10, UserId="Admin"},
               new Product {Id=15, ProductName="Charging System - Black", CategoryId = 2, Price = 25.99, Image=imageUrl, Condition="Нов", Discount=10, UserId="Admin"},
               new Product {Id=16, ProductName="Wii Remote Plus", CategoryId = 2, Price = 39.99, Image=imageUrl, Condition="Нов", Discount=10, UserId="Admin"},
               new Product {Id=17, ProductName="Fight Pad", CategoryId = 2, Price = 16.99, Image=imageUrl, Condition="Нов", Discount=10, UserId="Admin"},
               new Product {Id=18, ProductName="GameCube Controller", CategoryId = 2, Price = 29.99, Image=imageUrl, Condition="Нов", Discount=10, UserId="Admin"},
               new Product {Id=19, ProductName="FIFA 2016", CategoryId = 3, Price = 59.99, Image=imageUrl, Condition="Нов", Discount=10, UserId="Admin"},
               new Product {Id=20, ProductName="Need for Speed", CategoryId = 3, Price = 32.99, Image=imageUrl, Condition="Нов", Discount=10, UserId="Admin"},
               new Product {Id=21, ProductName="Call Of Duty", CategoryId = 3, Price = 36.99, Image=imageUrl, Condition="Нов", Discount=10, UserId="Admin"},
               new Product {Id=22, ProductName="Evolve", CategoryId = 3, Price = 49.99, Image=imageUrl, Condition="Нов", Discount=10, UserId="Admin"},
            };

            return products;
        }
    }
}

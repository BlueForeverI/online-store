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
            var user = new AppUser { UserName = "admin", Email = "admin@store.com", PasswordHash = hasher.HashPassword("admin"), Membership = "Admin", EmailConfirmed = true };
            var role = context.Roles.Where(r => r.Name == "Admin").First();
            user.Roles.Add(new IdentityUserRole { RoleId = role.Id, UserId = user.Id });
            context.Users.Add(user);
            user = new AppUser { UserName = "regular", Email = "regular@store.com", PasswordHash = hasher.HashPassword("regular"), Membership = "Regular", EmailConfirmed = true };
            role = context.Roles.Where(r => r.Name == "Regular").First();
            user.Roles.Add(new IdentityUserRole { RoleId = role.Id, UserId = user.Id });
            context.Users.Add(user);
            user = new AppUser { UserName = "advanced", Email = "advanced@store.com", PasswordHash = hasher.HashPassword("advanced"), Membership = "Advanced", EmailConfirmed = true };
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
            var products = new List<Product> {
               new Product {ProductName="PlayStation 4 Slim 500GB", CategoryId = 1, Price = 599.00, 
                   Image="https://cdn.ozone.bg/media/catalog/product/cache/1/image/a4e40ebdc3e371adff845072e1c73f37/s/l/4cd29b5070fd81c5a088b53d8e7bdb2e/playstation-4-slim-500gb-30.jpg", Condition="Нов", Discount=0, UserId="Admin"},
               new Product {ProductName="Nintendo Switch - Red & Blue", CategoryId = 1, Price = 659.00, 
                   Image="https://cdn.ozone.bg/media/catalog/product/cache/1/image/a4e40ebdc3e371adff845072e1c73f37/n/e/7efd72daeaba5ddaf05159341651caff/nintendo-switch---red-and-blue-30.jpg", Condition="Нов", Discount=10, UserId="Admin"},
               new Product {ProductName="XBOX ONE S 1TB", CategoryId = 1, Price = 599.00, 
                   Image="https://www.technopolis.bg/medias/sys_master/hf1/hea/12108789678110.jpg", Condition="Нов", Discount=10, UserId="Admin"},
               new Product {ProductName="PlayStation 5", CategoryId = 1, Price = 999.00, 
                   Image="https://www.technopolis.bg/medias/sys_master/h72/hdb/13551683567646.jpg", Condition="Нов", Discount=0, UserId="Admin"},

               new Product {ProductName="Xbox Controller", CategoryId = 2, Price = 239, 
                   Image="https://cdn.ozone.bg/media/catalog/product/cache/1/image/a4e40ebdc3e371adff845072e1c73f37/k/o/ba5f8b4ced497edac5d06ad6397a1397/konroler-razer---wolverine-v2--za-xbox-series-x-30.jpg", Condition="Нов", Discount=0, UserId="Admin"},
               new Product {ProductName="Гейминг слушалки Razer Kraken X - Multi-Platform", CategoryId = 2, Price = 119, 
                   Image="https://cdn.ozone.bg/media/catalog/product/cache/1/image/a4e40ebdc3e371adff845072e1c73f37/r/a/8b3e1b2b89fa331947375aff7110aa12/geyming-slushalki-razer-kraken-x---multi-platform-30.jpg", Condition="Нов", Discount=34, UserId="Admin"},
               new Product {ProductName="Гейминг мишка Razer Mamba Elite", CategoryId = 2, Price = 199, 
                   Image="https://cdn.ozone.bg/media/catalog/product/cache/1/image/a4e40ebdc3e371adff845072e1c73f37/r/a/c3ffee910e3ad051f51a6728e05f07a5/geyming-mishka-razer-mamba-elite-33.jpg", Condition="Нов", Discount=0, UserId="Admin"},
               new Product {ProductName="Гейминг клавиатура Genesis RHOD 400 RGB -NKG-0873", CategoryId = 2, Price = 45.00, 
                   Image="https://cdn.ozone.bg/media/catalog/product/cache/1/image/a4e40ebdc3e371adff845072e1c73f37/g/e/52a3eb61d07fbee2087f3b6ac0014395/geyming-klaviatura-genesis-rhod-400-rgb--nkg-0873---mnogotsvetna-podsvetka-31.jpg", Condition="Нов", Discount=0, UserId="Admin"},
               new Product {ProductName="Гейминг слушалки Razer Kraken Tournament Edition - Green", CategoryId = 2, Price = 199.99, 
                   Image="https://cdn.ozone.bg/media/catalog/product/cache/1/image/a4e40ebdc3e371adff845072e1c73f37/q/a/bcac3065a39a82a289cf41566b98c1b4/geyming-slushalki-razer-kraken-tournament-edition---green-33.jpg", Condition="Нов", Discount=35, UserId="Admin"},
               new Product {ProductName="Гейминг подложка Razer - Sphex Mini V2", CategoryId = 2, Price = 19.90, 
                   Image="https://cdn.ozone.bg/media/catalog/product/cache/1/image/a4e40ebdc3e371adff845072e1c73f37/r/a/4b28523c2d9777bed6f769b1c531c261/geyming-podlozhka-razer---sphex-mini-v2--m--tvarda--mnogotsvetna-31.jpg", Condition="Нов", Discount=0, UserId="Admin"},
               new Product {ProductName="Волан с педали Logitech - G29, за PC и PS4/PS5", CategoryId = 2, Price = 749.99, 
                   Image="https://cdn.ozone.bg/media/catalog/product/cache/1/image/a4e40ebdc3e371adff845072e1c73f37/g/2/0bf69b790549e3c6fdc58f4b9cb62053/volan-s-pedali-logitech---g29--za-pc-i-ps4-ps5--cheren-31.jpg", Condition="Нов", Discount=37, UserId="Admin"},
               new Product {ProductName="Комплект мишка и клавиатура Razer Abyssus Lite", CategoryId = 2, Price = 159.99, 
                   Image="https://cdn.ozone.bg/media/catalog/product/cache/1/image/a4e40ebdc3e371adff845072e1c73f37/g/a/9c4b507a559deafaaa8a4771bf522a8e/komplekt-mishka-i-klaviatura-razer-abyssus-lite-i-razer-cynosa-lite-31.jpg", Condition="Нов", Discount=48, UserId="Admin"},

               new Product {ProductName="Cyberpunk 2077 - Day One Edition (PS4)", CategoryId = 3, Price = 119.99, 
                   Image="https://cdn.ozone.bg/media/catalog/product/cache/1/image/a4e40ebdc3e371adff845072e1c73f37/c/p/7824b92c33afc74816e905ff7b108ce0/cyberpunk-2077---day-one-edition-ps4-30.jpg", Condition="Нов", Discount=49, UserId="Admin"},
               new Product {ProductName="Assassin's Creed Odyssey (PS4)", CategoryId = 3, Price = 59.99, 
                   Image="https://cdn.ozone.bg/media/catalog/product/cache/1/image/a4e40ebdc3e371adff845072e1c73f37/a/s/2b73fd848e8aac621dea3fc76f950a77/assassin-s-creed-odyssey-ps4-30.jpg", Condition="Нов", Discount=33, UserId="Admin"},
               new Product {ProductName="Doom Eternal (PS4)", CategoryId = 3, Price = 119.99, 
                   Image="https://cdn.ozone.bg/media/catalog/product/cache/1/image/a4e40ebdc3e371adff845072e1c73f37/d/o/dfb4efa7f9c40ef0d4b3d51408255cec/doom-eternal-ps4-30.jpg", Condition="Нов", Discount=55, UserId="Admin"},
               new Product {ProductName="Grand Theft Auto V - Premium Edition (PS4)", CategoryId = 3, Price = 59.99, 
                   Image="https://cdn.ozone.bg/media/catalog/product/cache/1/image/a4e40ebdc3e371adff845072e1c73f37/g/t/f9d05fbef6de6f819dfda7df869e7000/grand-theft-auto-v---premium-edition-ps4-30.jpg", Condition="Нов", Discount=33, UserId="Admin"},
            };

            return products;
        }
    }
}

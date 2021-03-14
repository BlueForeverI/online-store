using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineStore.Domain.Model;
using Microsoft.AspNet.Identity.EntityFramework;
using OnlineStore.Domain.Identity;

namespace OnlineStore.Domain.Infrastructure
{
    public partial class OnlineStoreDBContext : IdentityDbContext<AppUser>
    {
        public OnlineStoreDBContext()
            : base("name=OnlineStoreDBContext", throwIfV1Schema: false)
        {
            Database.SetInitializer<OnlineStoreDBContext>(new IdentityDbInitializer());
        }

        public static OnlineStoreDBContext Create()
        {
            return new OnlineStoreDBContext();
        }

        //public virtual DbSet<Role> Roles { get; set; }
        //public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
    }
}

using System;
using System.Data.Entity;
using OnlineStore.Domain.Model;
using Microsoft.AspNet.Identity.EntityFramework;
using OnlineStore.Domain.Identity;
using GameStore.Domain.Model;

namespace OnlineStore.Domain.Infrastructure
{
    public partial class OnlineStoreDBContext : IdentityDbContext<AppUser>
    {
        public OnlineStoreDBContext()
            : base("name=OnlineStoreDBContext", throwIfV1Schema: false)
        {
            Database.SetInitializer<OnlineStoreDBContext>(new IdentityDbInitializer());
        }

        public override int SaveChanges()
        {
            OnBeforeSaving();
            return base.SaveChanges();
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            var utcNow = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                if (entry.Entity is BaseEntity trackable)
                {
                    string operation = entry.State.ToString();
                    trackable.UpdatedOn = utcNow;
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            operation = "UPDATE";
                            break;

                        case EntityState.Added:
                            operation = "INSERT";
                            break;
                        case EntityState.Deleted:
                            operation = "DELETE";
                            break;
                    }

                    if (entry.State != EntityState.Unchanged)
                    {
                        Logs.Add(new Log
                        {
                            TableName = trackable.GetType().Name,
                            Operation = operation,
                            ExecutedAt = DateTime.UtcNow
                        });
                    }
                }
            }
        }

        public static OnlineStoreDBContext Create()
        {
            return new OnlineStoreDBContext();
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
    }
}

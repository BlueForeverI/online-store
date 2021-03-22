using GameStore.Domain.Model;
using OnlineStore.Domain.Infrastructure;
using OnlineStore.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace OnlineStore.Services
{
    public class BaseService<T> where T : BaseEntity
    {
        public List<T> GetAll()
        {
            using(var ctx = new OnlineStoreDBContext())
            {
                return ctx.Set<T>().ToList();
            }
        }

        public T Get(int id)
        {
            using (var ctx = new OnlineStoreDBContext())
            {
                return ctx.Set<T>().Find(id);
            }
        }

        public T Add(T entity)
        {
            using (var ctx = new OnlineStoreDBContext())
            {
                var saved = ctx.Set<T>().Add(entity);
                ctx.SaveChanges();
                return saved;
            }
        }

        public T Update(T entity)
        {
            using (var ctx = new OnlineStoreDBContext())
            {
                ctx.Set<T>().AddOrUpdate(entity);
                ctx.SaveChanges();
                return entity;
            }
        }

        public void Delete(int id)
        {
            using (var ctx = new OnlineStoreDBContext())
            {
                ctx.Set<T>().Remove(ctx.Set<T>().Find(id));
                ctx.SaveChanges();
            }
        }

        public bool Exists(Func<T, bool> predicate)
        {
            using (var ctx = new OnlineStoreDBContext())
            {
                return ctx.Set<T>().Any(predicate);
            }
        }

        public int Count()
        {
            using (var ctx = new OnlineStoreDBContext())
            {
                return ctx.Set<T>().Count();
            }
        }
    }
}

using Microsoft.AspNet.Identity.EntityFramework;
using OnlineStore.Domain.Identity;
using OnlineStore.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Services
{
    public class UserService
    {
        public void Register(string email, string password, string username, string memberRole)
        {
            using (OnlineStoreDBContext context = new OnlineStoreDBContext())
            {
                Domain.Helper.PasswordHasher hasher = new Domain.Helper.PasswordHasher();
                var user = new AppUser
                {
                    UserName = username,
                    Email = email,
                    PasswordHash = hasher.HashPassword(password),
                    Membership = memberRole
                };
                var role = context.Roles.Where(r => r.Name == memberRole).First();
                user.Roles.Add(new IdentityUserRole { RoleId = role.Id, UserId = user.Id });
                context.Users.Add(user);
                context.SaveChanges();
            }
        }
    }
}

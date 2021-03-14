using OnlineStore.Domain.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Domain.Identity
{
    public class AppUserStore : UserStore<AppUser>
    {
        public AppUserStore(OnlineStoreDBContext context) : base(context)
        {

        }
    }
}

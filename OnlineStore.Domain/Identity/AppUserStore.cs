using OnlineStore.Domain.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;

namespace OnlineStore.Domain.Identity
{
    public class AppUserStore : UserStore<AppUser>
    {
        public AppUserStore(OnlineStoreDBContext context) : base(context)
        {

        }
    }
}

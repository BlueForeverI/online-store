using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Services.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Membership { get; set; }
    }
}
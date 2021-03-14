using OnlineStore.Domain.Identity;
using OnlineStore.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore.WebUI.Areas.Admin.Models
{
    public class RoleViewModel
    {
        public String Id { get; set; }
        [Required]
        public String Name { get; set; }
        [Required]
        public String Description { get; set; }
    }
}
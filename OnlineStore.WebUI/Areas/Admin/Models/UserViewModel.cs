﻿using OnlineStore.Domain.Identity;
using OnlineStore.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore.WebUI.Areas.Admin.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Потребителско име")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Роля")]
        public string Membership { get; set; }
    }
}
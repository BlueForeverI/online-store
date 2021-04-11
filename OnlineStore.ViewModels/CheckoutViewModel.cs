using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore.ViewModels
{
    public class CheckoutViewModel
    {
        [Required]
        [Display(Name = "Име и фамилия")]
        public string FullName { get; set; }
        [Required]
        [Display(Name = "Адрес")]
        public string Address { get; set; }
        [Required]
        [Display(Name = "Град")]
        public string City { get; set; }
        [Required]
        [Display(Name = "Пощенски код")]
        public string Zip { get; set; }
    }
}
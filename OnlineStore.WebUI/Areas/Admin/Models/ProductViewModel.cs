﻿using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.WebUI.Areas.Admin.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Име")]
        [Required]
        public string ProductName { get; set; }
        [Display(Name = "Категория")]
        public int CategoryId { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public string Condition { get; set; }
        [Required]
        public int Discount { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
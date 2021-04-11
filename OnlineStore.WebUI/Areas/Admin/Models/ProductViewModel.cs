using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.WebUI.Areas.Admin.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Име")]
        [Required(ErrorMessage = "Полето Име е задължително")]
        public string ProductName { get; set; }
        [Display(Name = "Категория")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Полето Цена е задължително")]
        [Display(Name = "Цена")]
        public double Price { get; set; }
        [Required(ErrorMessage = "Полето Снимка е задължително")]
        [Display(Name = "Снимка")]
        public string Image { get; set; }
        [Required(ErrorMessage = "Полето Състояние е задължително")]
        [Display(Name = "Състояние (нов/използван)")]
        public string Condition { get; set; }
        [Required(ErrorMessage = "Полето Отстъпка е задължително")]
        [Display(Name = "Отстъпка %")]
        public int Discount { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
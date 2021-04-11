using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.WebUI.Areas.Admin.Models
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Полето Име е задължително")]
        public String CategoryName { get; set; }
    }
}
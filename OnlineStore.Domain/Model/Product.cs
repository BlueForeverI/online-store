using GameStore.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Domain.Model
{
    public class Product : BaseEntity
    {
        public Product()
        {
            this.Reviews = new HashSet<Review>();
        }
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
        [Display(Name = "Състояние")]
        public string Condition { get; set; }
        [Required(ErrorMessage = "Полето Отстъпка е задължително")]
        [Display(Name = "Отстъпка")]
        public int Discount { get; set; }
        [Required(ErrorMessage = "Полето Потребител е задължително")]
        public string UserId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}

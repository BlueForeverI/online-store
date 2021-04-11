using GameStore.Domain.Model;
using OnlineStore.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Domain.Model
{
    public class Review : BaseEntity
    {
        public Review()
        {

        }
        [Required(ErrorMessage = "Полето Продукт е задължително")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Полето Потребител е задължително")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Полето Рейтинг е задължително")]
        public int Rating { get; set; }
        public string Comments { get; set; }
        [Required(ErrorMessage = "Полето Дата е задължително")]
        public DateTime ReviewDate { get; set; }
        public virtual Product Product { get; set; }
        public virtual AppUser User { get; set; }
    }
}

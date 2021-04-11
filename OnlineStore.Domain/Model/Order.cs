using GameStore.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Domain.Model
{
    public class Order : BaseEntity
    {
        public Order()
        {
            this.OrderItems = new HashSet<OrderItem>();
        }
        [Required(ErrorMessage = "Полето Потребител е задължително")]
        public string UserId { get; set; }
        [Required(ErrorMessage = "Полето Име е задължително")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Полето Адрес е задължително")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Полето Град е задължително")]
        public string City { get; set; }
        [Required(ErrorMessage = "Полето Област е задължително")]
        public string State { get; set; }
        [Required(ErrorMessage = "Полето Пощенски код е задължително")]
        public string Zip { get; set; }
        [Required(ErrorMessage = "Полето Номер за потвърждение е задължително")]
        public string ConfirmationNumber { get; set; }
        [Required(ErrorMessage = "Полето Дата за доставка е задължително")]
        public DateTime DeliveryDate { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}

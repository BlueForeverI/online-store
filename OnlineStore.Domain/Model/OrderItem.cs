using GameStore.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Domain.Model
{
    public class OrderItem : BaseEntity
    {
        public OrderItem()
        {

        }
        [Required(ErrorMessage = "Полето ID е задължително")]
        public int OrderId { get; set; }
        [Required(ErrorMessage = "Полето Продукт е задължително")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Полето Количество е задължително")]
        public int Quantity { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}

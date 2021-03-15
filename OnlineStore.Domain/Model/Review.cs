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
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int Rating { get; set; }
        public string Comments { get; set; }
        [Required]
        public DateTime ReviewDate { get; set; }
        public virtual Product Product { get; set; }
        public virtual AppUser User { get; set; }
    }
}

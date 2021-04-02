﻿using GameStore.Domain.Model;
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
        [Required]
        public string UserId { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Zip { get; set; }
        [Required]
        public string ConfirmationNumber { get; set; }
        [Required]
        public DateTime DeliveryDate { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}

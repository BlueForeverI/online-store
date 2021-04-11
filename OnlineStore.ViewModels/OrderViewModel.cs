using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.ViewModels
{
    public class OrderViewModel
    {
        [Display(Name = "ID")]
        public int OrderId { get; set; }
        [Display(Name = "Потребител")]
        public string UserId { get; set; }
        [Display(Name = "Потребителско име")]
        public string UserName { get; set; }
        [Display(Name = "Име")]
        public string FullName { get; set; }
        [Display(Name = "Адрес")]
        public string Address { get; set; }
        [Display(Name = "Град")]
        public string City { get; set; }
        [Display(Name = "Пощенски код")]
        public string Zip { get; set; }
        [Display(Name = "Номер за потвърждение")]
        public string ConfirmationNumber { get; set; }
        [Display(Name = "Дата на доставка")]
        public DateTime DeliveryDate { get; set; }
        public List<OrderItemViewModel> Items { get; set; }
    }
}
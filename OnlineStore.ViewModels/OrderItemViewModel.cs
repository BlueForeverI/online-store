using System.ComponentModel.DataAnnotations;

namespace OnlineStore.ViewModels
{
    public class OrderItemViewModel
    {
        [Display(Name = "ID")]
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        [Display(Name = "Продукт")]
        public int ProductId { get; set; }
        [Display(Name = "Име на продукт")]
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        [Display(Name = "Категория")]
        public string CategoryName { get; set; }
        [Display(Name = "Цена")]
        public double Price { get; set; }
        [Display(Name = "Снимка")]
        public string Image { get; set; }
        [Display(Name = "Състояние")]
        public string Condition { get; set; }
        [Display(Name = "Отстъпка")]
        public int Discount { get; set; }
        [Display(Name = "Количество")]
        public int Quantity { get; set; }
        public double GetDiscountedPrice()
        {
            return Price * (100 - Discount) / 100;
        }
        public double GetTotalCost()
        {
            return Quantity * GetDiscountedPrice();
        }
    }
}
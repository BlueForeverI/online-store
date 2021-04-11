using System.ComponentModel.DataAnnotations;

namespace OnlineStore.ViewModels
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Полето Име и фамилия е задължително")]
        [Display(Name = "Име и фамилия")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Полето Адрес е задължително")]
        [Display(Name = "Адрес")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Полето Град е задължително")]
        [Display(Name = "Град")]
        public string City { get; set; }
        [Required(ErrorMessage = "Полето Пощенски код е задължително")]
        [Display(Name = "Пощенски код")]
        public string Zip { get; set; }
    }
}
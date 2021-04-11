using System.ComponentModel.DataAnnotations;

namespace OnlineStore.WebUI.Areas.Admin.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Полето Email е задължително")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Полето Потребителско име е задължително")]
        [Display(Name = "Потребителско име")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Полето Роля е задължително")]
        [Display(Name = "Роля")]
        public string Membership { get; set; }
    }
}
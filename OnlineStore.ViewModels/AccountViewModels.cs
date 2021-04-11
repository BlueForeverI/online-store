using System.ComponentModel.DataAnnotations;

namespace OnlineStore.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Полето Email е задължително")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Полето Парола е задължително")]
        [StringLength(100, ErrorMessage = "{0} трябва да е поне {2} символа.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Потвърди паролата")]
        [Compare("Password", ErrorMessage = "Паролите не съвпадат.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Полето Потребителско име е задължително")]
        [Display(Name = "Потребителско име")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Полето Роля е задължително")]
        [Display(Name = "Роля")]
        public string Membership { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Полето Email е задължително")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Полето Парола е задължително")]
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        public string Password { get; set; }

        [Display(Name = "Запомни ме")]
        public bool RememberMe { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Полето Текуща парола е задължително")]
        [DataType(DataType.Password)]
        [Display(Name = "Текуща парола")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Полето Парола е задължително")]
        [StringLength(100, ErrorMessage = "{0} трябва да е поне {2} символа.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Потвърди новата парола")]
        [Compare("NewPassword", ErrorMessage = "Паролите не съвпадат.")]
        public string ConfirmPassword { get; set; }
    }
}
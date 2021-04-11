using System.ComponentModel.DataAnnotations;

namespace OnlineStore.WebUI.Areas.Admin.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Полето Име е задължително")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Полето Описание е задължително")]
        public string Description { get; set; }
    }
}
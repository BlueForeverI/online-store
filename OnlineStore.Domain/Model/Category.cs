using GameStore.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Domain.Model
{
    public class Category : BaseEntity
    {
        public Category()
        {

        }
        [Display(Name = "Категория")]
        [Required(ErrorMessage = "Полето Име е задължително")]
        public string CategoryName { get; set; }
    }
}

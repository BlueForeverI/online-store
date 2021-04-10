using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Domain.Identity
{
    [MetadataType(typeof(RoleMetaData))]
    public class AppRole : IdentityRole
    {
        public AppRole() : base() { }

        public AppRole(string description)
            : base()
        {
            this.Description = description;
        }

        public virtual string Description { get; set; }
    }

    public class RoleMetaData
    {
        [Required(ErrorMessage = "Моля въведете име на роля")]
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Моле въведете описание")]
        public string Description { get; set; }
    }
}

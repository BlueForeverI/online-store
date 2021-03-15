using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Domain.Model
{
    public class BaseEntity
    {
        [Required]
        public int Id { get; set; }
        [Column("UpdatedOn_17118060")]
        public DateTime UpdatedOn { get; set; }
    }
}

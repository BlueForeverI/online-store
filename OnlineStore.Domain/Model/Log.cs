using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Domain.Model
{
    [Table("log_17118060")]
    public class Log
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public string Operation { get; set; }
        public DateTime ExecutedAt { get; set; }
    }
}

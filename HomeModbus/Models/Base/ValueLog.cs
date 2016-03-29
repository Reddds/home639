using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeModbus.Models.Base
{
    public class ValueLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]public long Id { get; set; }
        public DateTime Time { get; set; }
        public string ParameterId { get; set; }
        public double Value { get; set; }
    }

    public class ValueLogContext : DbContext
    {
        public DbSet<ValueLog> ValueLogs { get; set; }
    }
}

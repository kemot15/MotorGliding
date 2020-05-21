using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Models.Db
{
    public class Calendar
    {
        public int Id { get; set; }
        public DateTime Present { get; set; }
        
    }
}

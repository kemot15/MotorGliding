using MotorGliding.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Models.ViewModels
{
    public class CalendarViewModel
    {
        public DateTime DateTime { get; set; }
        public IList<Calendar> Calendar { get; set; }
        
        public int EventId { get; set; }
    }
}

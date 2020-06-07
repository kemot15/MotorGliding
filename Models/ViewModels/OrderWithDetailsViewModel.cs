using MotorGliding.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Models.ViewModels
{
    public class OrderWithDetailsViewModel
    {
        public Order Order { get; set; }
        public IList<Event> EventList { get; set; }
    }
}

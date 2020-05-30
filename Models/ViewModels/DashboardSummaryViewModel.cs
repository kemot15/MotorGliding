using MotorGliding.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Models.ViewModels
{
    public class DashboardSummaryViewModel
    {
        public IList<Event> Events { get; set; }
        public IList<Order> Orders { get; set; }
    }
}

using MotorGliding.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Models.ViewModels
{
    public class MainPageViewModel
    {
       // public RegLogViewModel RegLogViewModel { get; set; }
        public IList<Vehicle> Vehicle { get; set; }
        public IList<Event> Events { get; set; }
        public EmailViewModel Email { get; set; }
    }
}

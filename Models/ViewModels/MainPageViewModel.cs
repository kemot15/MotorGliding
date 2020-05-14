using MotorGliding.Migrations;
using MotorGliding.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Models.ViewModels
{
    public class MainPageViewModel
    {
        public RegLogViewModel RegLogViewModel { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}

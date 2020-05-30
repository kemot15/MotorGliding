using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Models.ViewModels
{
    public class RegLogViewModel
    {
        public LoginViewModel LoginViewModel { get; set; }
        public RegistrationViewModel RegistrationViewModel { get; set; }
        public Tab ActiveTab { get; set; }
    }

    public enum Tab
    {
        Login,
        Registration
    }
}

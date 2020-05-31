using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Login jest wymagany")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}

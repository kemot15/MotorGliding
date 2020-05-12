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
        [Required(ErrorMessage = "Email jest wymagany")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Hasło jest wymagane")]
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}

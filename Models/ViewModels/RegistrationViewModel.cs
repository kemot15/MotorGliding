using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Models.ViewModels
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "Pole wymagane")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Pole wymagane")]
        [EmailAddress(ErrorMessage = "Nie poprawny adres email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Pole wymagane")]
        [PasswordPropertyText]
        public string Password { get; set; }
        [Required(ErrorMessage = "Pole wymagane")]
        [Compare("Password", ErrorMessage ="Podane hasła są różne")]
        [PasswordPropertyText]
        public string RepeatPassword { get; set; }
    }
}

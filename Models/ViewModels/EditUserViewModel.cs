using Microsoft.AspNetCore.Identity;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;

namespace MotorGliding.Models.ViewModels
{
    public class EditUserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [DisplayName("Imię"), MaxLength(255)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [DisplayName("Nazwisko"), MaxLength(255)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [DisplayName("Numer telefonu"), MaxLength(15)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [DisplayName("Ulica"), MaxLength(50)]   
        public string Street { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [DisplayName("Kod pocztowy"), MaxLength(7)]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [DisplayName("Miasto"), MaxLength(255)]
        public string City { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [DisplayName("Państwo"), MaxLength(255)]
        public string Country { get; set; }
        public int AddressId { get; set; }
        public string Email { get; set; }
        public int OrderId { get; set; }

        //public IList<IdentityRole> RoleList { get; set; }
    }
}

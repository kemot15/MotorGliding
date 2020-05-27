using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;

namespace MotorGliding.Models.ViewModels
{
    public class EditUserViewModel
    {
        public int Id { get; set; }
        [DisplayName("Imię"), MaxLength(255)]
        public string Name { get; set; }
        [DisplayName("Nazwisko"), MaxLength(255)]
        public string LastName { get; set; }
        [DisplayName("Numer telefonu"), MaxLength(15)]
        public string PhoneNumber { get; set; }
        [DisplayName("Ulica"), MaxLength(50)]
        public string Street { get; set; }
        [DisplayName("Kod pocztowy"), MaxLength(7)]
        public string ZipCode { get; set; }
        [DisplayName("Miasto"), MaxLength(255)]
        public string City { get; set; }
        [DisplayName("Państwo"), MaxLength(255)]
        public string Country { get; set; }
        public int AddressId { get; set; }
        public string Email { get; set; }
        public int OrderId { get; set; }
    }
}

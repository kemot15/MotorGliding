using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Models.Db
{
    public class OrderUser
    {
        public int Id { get; set; }
        [DisplayName("Imię"), MaxLength(255)]
        public string Name { get; set; }
        [DisplayName("Nazwisko"), MaxLength(255)]
        public string LastName { get; set; }
        [DisplayName("Numer telefonu"), MaxLength(15)]
        public string PhoneNumber { get; set; }
        [DisplayName("E-mail"), MaxLength(255)]
        public string Email { get; set; }

        [ForeignKey("AddressId")]
        public Address Address { get; set; }
        public int OrderId { get; set; }
        public int UserId { get; set; }

    }
}

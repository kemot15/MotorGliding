using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Models.Db
{
    public class User : IdentityUser<int>
    {
        [DisplayName("Imię"), MaxLength(255)]
        public string Name { get; set; }
        [DisplayName("Nazwisko"),MaxLength(255)]
        public string LastName { get; set; }
        [ForeignKey("AddressId")]
        public Address Address { get; set; }
    }
}

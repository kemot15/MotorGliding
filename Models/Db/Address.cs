using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace MotorGliding.Models.Db
{
    public class Address
    {
        public int Id { get; set; }
        [Required, MaxLength(255), DisplayName("Ulica")]
        public string Street { get; set; }
        [Required, MaxLength(255), DisplayName("Kod pocztowy")]
        public string ZipCode { get; set; }
        [Required, MaxLength(255), DisplayName("Miejscowość")]
        public string City { get; set; }
        [Required, MaxLength(255), DisplayName("Państwo")]
        public string Country { get; set; }
        public User User { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace MotorGliding.Models.Db
{
    public class Vehicle
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        [DisplayName("Nazwa")]
        public string Name { get; set; }
        [Required, MaxLength]
        [DisplayName("Opis")]
        public string Description { get; set; }
        [DefaultValue(false)]
        [DisplayName ("Aktywny")]
        public bool Visible { get; set; }
        public ICollection<Image> Images { get; set; }
        public ICollection<Features> Features { get; set; }
        [NotMapped]
        public Image Image { get; set; }
        [NotMapped]
        [Range(0, 100, ErrorMessage = "Wartść musi być z przedziału 0-100")]
        public Features Features1 { get; set; }
        [NotMapped]
        [Range(0, 100, ErrorMessage = "Wartść musi być z przedziału 0-100")]
        public Features Features2 { get; set; }
        [NotMapped]
        [Range(0, 100, ErrorMessage = "Wartść musi być z przedziału 0-100")]
        public Features Features3 { get; set; }
        [NotMapped]
        [Range(0, 100, ErrorMessage = "Wartść musi być z przedziału 0-100")]
        public Features Features4 { get; set; }
    }
}

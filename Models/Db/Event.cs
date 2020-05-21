using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace MotorGliding.Models.Db
{
    public class Event
    {
        public int Id { get; set; }
        [Required, StringLength(100)]
        [DisplayName("Tytuł")]
        public string Title { get; set; }
        [StringLength(200)]
        [DisplayName("Opis")]
        public string Description { get; set; }
        [MaxLength]
        public string Info { get; set; }
        [Column(TypeName ="Money")]
        [Range(0, int.MaxValue, ErrorMessage = "Cena musi być dodatnia")]
        [DisplayName("Cena")]
        public double Price { get; set; }


        [Column(TypeName = "Money")]
        [Range(0, int.MaxValue, ErrorMessage = "Cena musi być dodatnia")]
        [DisplayName("Cena filmowania")]
        public double CameraPrice { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Wartść musi być dodatnia")]

        [DisplayName("Długość lotu")]
        public int Duration { get; set; }
        public DateTime CreateData { get; set; } = DateTime.Now;
        public ICollection<Image> Gallery { get; set; }
        [DisplayName("Aktywne")]
        public bool Visible { get; set; } = true;
        //[NotMapped]
        //public OrderDetails OrderDetails { get; set; }

        public string ShowText (string text)
        {
            var result = "";
            for (int i = 0; i < 5; i++)
            {
                result += $"<h1>{text}</h1>";
            }
            return result;
        }
    }
}

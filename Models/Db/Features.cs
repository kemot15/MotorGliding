using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Models.Db
{
    public class Features
    {
        public int Id { get; set; }
        [StringLength(15)]
        public string Name { get; set; }
        [Range(0, 100, ErrorMessage = "Wartość {0} musi być pomiędzy {1} i {2}.")]
        public int Value { get; set; }
        //id czego dotycza atrybuty - np id dodanego pojazdu
        public int SourceId { get; set; }
        
    }
}

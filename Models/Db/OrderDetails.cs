using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Models.Db
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public double CameraPrice { get; set; }
        public int OrderId { get; set; }

        public Order Order { get; set; }
        public int EventID { get; set; }
        
        public bool Camera { get; set; }
        public int Quantity { get; set; }
        [NotMapped]
        public virtual string EventTitle { get; set; }

    }
}

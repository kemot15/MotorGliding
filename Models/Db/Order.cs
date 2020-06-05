using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace MotorGliding.Models.Db
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreateData { get; set; } = DateTime.Now;
        //public int? OrderUserId { get; set; }
        public OrderUser OrderUser { get; set; }
        public bool Accepted { get; set; }
        public ICollection<OrderDetails> OrderDetails { get; set; }
    }
}

using MotorGliding.Models.Db;
using MotorGliding.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Services.OrderFilter
{
    public class LastNameOrderFilter : IOrderFilter
    {
       // public string filter { get; set; }
        public IOrderFilter Successor { get; set; }

        public LastNameOrderFilter( IOrderFilter successor)
        {         
            Successor = successor;
        }

        public IList<Order> FilterResult(IList<Order> orders, DashboardSummaryViewModel model)
        {
            IList<Order> result = orders;
            if (!string.IsNullOrWhiteSpace(model.LastName))
                result = orders.Where(o => o.OrderUser.LastName.ToUpper().Contains(model.LastName.ToUpper())).ToList();
            if (Successor != null)
                return Successor.FilterResult(result,model);
            return result;
        }
    }
}

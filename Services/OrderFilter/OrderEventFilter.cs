using MotorGliding.Models.Db;
using MotorGliding.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Services.OrderFilter
{
    public class OrderEventFilter : IOrderFilter
    {
        public IOrderFilter Successor { get; set; }

        public OrderEventFilter(IOrderFilter successor)
        {
            Successor = successor;
        }

        public IList<Order> FilterResult(IList<Order> orders, DashboardSummaryViewModel model)
        {
            IList<Order> result = orders;
            if (model.Event != "0")
                result = orders.Where(o => o.OrderDetails.Any(o => o.EventID.ToString() == model.Event)).ToList();  //Sprawdzic co jest w modelu? czy w Evencie jest tylko ID?

            if (Successor != null)
                return Successor.FilterResult(result, model);
            return result;
        }
    }
}

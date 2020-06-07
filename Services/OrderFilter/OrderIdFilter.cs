using MotorGliding.Models.Db;
using MotorGliding.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Services.OrderFilter
{
    public class OrderIdFilter : IOrderFilter
    {
        public IOrderFilter Successor { get; set; }

        public OrderIdFilter(IOrderFilter successor)
        {
            Successor = successor;
        }

        public IList<Order> FilterResult(IList<Order> orders, DashboardSummaryViewModel model)
        {
            IList<Order> result = orders;
            if (model.OrderID != 0)
                result = orders.Where(o => o.Id == model.OrderID).ToList();
            if (Successor != null)
                return Successor.FilterResult(result, model);
            return result;
        }
    }
}


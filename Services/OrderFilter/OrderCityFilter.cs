using MotorGliding.Models.Db;
using MotorGliding.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Services.OrderFilter
{
    public class OrderCityFilter : IOrderFilter
    {
        public IOrderFilter Successor { get; set; }

        public OrderCityFilter(IOrderFilter successor)
        {
            Successor = successor;
        }

        public IList<Order> FilterResult(IList<Order> orders, DashboardSummaryViewModel model)
        {
            IList<Order> result = orders;
            if (!string.IsNullOrWhiteSpace(model.City))
                result = orders.Where(o => o.OrderUser.Address.City.ToUpper().Contains(model.City.ToUpper())).ToList();
            if (Successor != null)
                return Successor.FilterResult(result,model);
            return result;
        }
    }
}

using MotorGliding.Models.Db;
using MotorGliding.Models.ViewModels;
using MotorGliding.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Services.OrderFilter
{
    public class OrderNameFilter : IOrderFilter
    {
        public IOrderFilter Successor { get; set ; }

        public OrderNameFilter(IOrderFilter successor)
        {
            Successor = successor;
        }

        public IList<Order> FilterResult(IList<Order> orders, DashboardSummaryViewModel model)
        {
            IList<Order> result = orders;
            if (!string.IsNullOrWhiteSpace(model.Name))
                result = orders.Where(o => o.OrderUser.Name.ToUpper().Contains(model.Name.ToUpper())).ToList() ;
            if (Successor != null)
                return Successor.FilterResult(result, model);
            return result;
        }       
    }
}

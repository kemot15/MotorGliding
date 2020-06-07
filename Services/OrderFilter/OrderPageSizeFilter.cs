using MotorGliding.Models.Db;
using MotorGliding.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Services.OrderFilter
{
    public class OrderPageSizeFilter : IOrderFilter
    {
        public IOrderFilter Successor { get; set; }

        public OrderPageSizeFilter(IOrderFilter successor)
        {
            Successor = successor;
        }

        public IList<Order> FilterResult(IList<Order> orders, DashboardSummaryViewModel model)
        {
            var pageSize = int.Parse(model.PageSize);
            IList<Order> result = orders;
            if (model.PageSize != "1")
                result = orders.Skip((model.Page-1) * pageSize).Take(pageSize).ToList();
            if (Successor != null)
                return Successor.FilterResult(result, model);
            return result;
        }
    }
}

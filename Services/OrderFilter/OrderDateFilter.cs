using MotorGliding.Models.Db;
using MotorGliding.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Services.OrderFilter
{
    public class OrderDateFilter : IOrderFilter
    {
        public IOrderFilter Successor { get; set; }

        public OrderDateFilter(IOrderFilter successor)
        {
            Successor = successor;
        }

        public IList<Order> FilterResult(IList<Order> orders, DashboardSummaryViewModel model)
        {
            IList<Order> result = orders;
            //if (model.dateFrom == default && model.dateTo == default)
                result = orders.Where(o => o.CreateData >= model.dateFrom && o.CreateData <= model.dateTo).ToList();
            if (Successor != null)
                return Successor.FilterResult(result, model);
            return result;
        }
    }
}

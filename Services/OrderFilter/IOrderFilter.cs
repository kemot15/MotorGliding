using MotorGliding.Models.Db;
using MotorGliding.Models.ViewModels;
using MotorGliding.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Services.OrderFilter
{
    public interface IOrderFilter
    {
        IOrderFilter Successor { get; set; }
        //string filter { get; set; }

        IList<Order> FilterResult(IList<Order> orders, DashboardSummaryViewModel model);
    }
}

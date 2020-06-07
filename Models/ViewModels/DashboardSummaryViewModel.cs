using Microsoft.AspNetCore.Mvc.Rendering;
using MotorGliding.Models.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Models.ViewModels
{
    public class DashboardSummaryViewModel
    {
        public IList<SelectListItem> Events { get; set; }
        [DisplayName("Wydarzenie")]
        public string Event { get; set; } = "0";
        public IList<Order> Orders { get; set; }
        [DisplayName("Od")]
        public DateTime DateFrom { get; set; } = DateTime.Now.AddDays(-1);
        [DisplayName("Do")]
        public DateTime DateTo { get; set; } = DateTime.Now.AddDays(30);
        [DisplayName("Imię")]
        public string Name { get; set; }
        [DisplayName("Nazwisko")]
        public string LastName { get; set; }
        [DisplayName("Miasto")]
        public string City { get; set; }
        [AllowNull]
        public int EventId { get; set; }

        public IList<SelectListItem> PageSizes { get; set; }
        [DisplayName("Ilość na stronie")]
        public string PageSize { get; set; } = "0";
        public int Page { get; set; }
        [DisplayName("Numer zamówienia")]
        public int OrderID { get; set; }

    }
}

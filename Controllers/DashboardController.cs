using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MotorGliding.Models.Db;
using MotorGliding.Models.ViewModels;
using MotorGliding.Services;
using MotorGliding.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using MotorGliding.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using MotorGliding.Services.OrderFilter;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MotorGliding.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ICalendarService _calendarService;
        private readonly IEventService _eventService;
        private readonly UserManager<User> UserManager;
        private readonly IOrderService _orderService;

        public DashboardController(ICalendarService calendarService, IEventService eventService, UserManager<User> userManager, IOrderService orderService)
        {
            _calendarService = calendarService;
            _eventService = eventService;
            UserManager = userManager;
            _orderService = orderService;
        }

        public async Task<IActionResult> Index(DashboardSummaryViewModel model, int page)
        {
            //if (page == 1 && page != model.Page)
            //    page = model.Page;
            if (page < 1) page = 1;
            model.Page = page;
            ViewBag.Active = Tabs.Dashboard;
            var events = await _eventService.ListAsync();
            //var selectListItem = events.Select(e => new SelectListItem(e.Title, .Name)).ToList();
            var orderFilter = new OrderNameFilter(new LastNameOrderFilter(new OrderCityFilter(new OrderDateFilter(new OrderEventFilter(new OrderPageSizeFilter(null))))));
            var eventList = events.Select(e => new SelectListItem() { Text = e.Title, Value = e.Id.ToString() }).ToList();
            eventList.Add(new SelectListItem { Text = "Wszystkie", Value = "0" });

            var pageSizes = new List<SelectListItem> { 
                 new SelectListItem { Text = "5", Value = "5" },
                  new SelectListItem { Text = "10", Value = "10" },
                   new SelectListItem { Text = "25", Value = "25" },
                    new SelectListItem { Text = "50", Value = "50" },
                new SelectListItem { Text = "Wszystkie", Value = "0" },
            };


            if (User.IsInRole("Admin"))
            {
                var summaryViewModel = new DashboardSummaryViewModel()
                {
                    PageSizes = pageSizes,
                    Events = eventList,
                    Orders = orderFilter.FilterResult(await _orderService.GetSummaryOrders(), model)
                };

                ViewBag.Page = page;
                ViewBag.PagesMax = Math.Ceiling((double)summaryViewModel.Orders.Count / double.Parse(summaryViewModel.PageSize));
                summaryViewModel.Page = page;
                return View(summaryViewModel);
            }
            return RedirectToAction("UserIndex");
            
        }

        public IActionResult ClearFilters()
        {
            return RedirectToAction("Index", new { model = new DashboardSummaryViewModel() });
        }

        public async Task<IActionResult> UserIndex()
        {
            var user = await UserManager.GetUserAsync(User);
            var orders = await _orderService.FilterOrderByUser(user.Id);
            return View(orders);
        }        

        public async Task<IActionResult> Calendar(DateTime dateTime)
        {
            if (dateTime == default)
                dateTime = DateTime.Now;
            var firstDayOfMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
            switch (firstDayOfMonth.DayOfWeek.ToString())
            {
                case "Tuesday":
                    firstDayOfMonth = firstDayOfMonth.AddDays(-1);
                    break;
                case "Wednesday":
                    firstDayOfMonth = firstDayOfMonth.AddDays(-2);
                    break;
                case "Thursday":
                    firstDayOfMonth = firstDayOfMonth.AddDays(-3);
                    break;
                case "Friday":
                    firstDayOfMonth = firstDayOfMonth.AddDays(-4);
                    break;
                case "Saturday":
                    firstDayOfMonth = firstDayOfMonth.AddDays(-5);
                    break;
                case "Sunday":
                    firstDayOfMonth = firstDayOfMonth.AddDays(-6);
                    break;
            }
            var model = new CalendarViewModel()
            {
                DateTime = firstDayOfMonth,
                Calendar = await _calendarService.GetReservationAsync(dateTime)
            };
            return View(model);
        }

        public async Task<IActionResult> ReserveDay(DateTime reserved, DateTime currentCalendarDate)
        {
            if (reserved != default)
                await _calendarService.ReserveDayAsync(reserved);
            return RedirectToAction("Calendar", new { dateTime = currentCalendarDate });
        }

        public async Task<IActionResult> CancelDay(int id, DateTime currentCalendarDate)
        {
            if (id != 0)
            {
                await _calendarService.CancelDayAsync(id);
            }
            return RedirectToAction("Calendar", new { dateTime = currentCalendarDate });
        }

        public async Task<IActionResult> CleareReserved(DateTime currentCalendarDate)
        {
            await _calendarService.ClearReservedAsync(currentCalendarDate);
            return RedirectToAction("Calendar", new { dateTime = currentCalendarDate });
        }
      

    }
}

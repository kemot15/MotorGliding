using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MotorGliding.Models.Db;
using MotorGliding.Models.ViewModels;
using MotorGliding.Services;
using MotorGliding.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using MotorGliding.Models.Enums;

namespace MotorGliding.Controllers
{
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

        public async Task<IActionResult> Index(int orderId, int eventId)
        {
            ViewBag.Active = Tabs.Dashboard;
            if (User.IsInRole("Admin"))
            {
                var summaryViewModel = new DashboardSummaryViewModel()
                {
                    Events = await _eventService.ListAsync(),
                    Orders = orderId == 0 ? await _orderService.GetSummaryOrders() : await _orderService.FilterOrderContainingEvent(orderId)
                };
                return View(summaryViewModel);
            }
            return RedirectToAction("UserIndex");
            
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

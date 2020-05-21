using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MotorGliding.Models.ViewModels;
using MotorGliding.Services;

namespace MotorGliding.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ICalendarService _calendarService;

        public DashboardController(ICalendarService calendarService)
        {
            _calendarService = calendarService;
        }

        public IActionResult Index()
        {
            ViewBag.Active = "Dashboard";
            return View();
        }

        public IActionResult Account()
        {
            ViewBag.Active = "Account";
            return View();
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

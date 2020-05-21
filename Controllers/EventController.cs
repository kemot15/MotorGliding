using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.IdentityModel.Tokens;
using MotorGliding.Models.Db;
using MotorGliding.Models.ViewModels;
using MotorGliding.Services;
using MotorGliding.Services.Interfaces;

namespace MotorGliding.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly ICalendarService _calendarService;

        public EventController(IEventService eventService, ICalendarService calendarService)
        {
            _eventService = eventService;
            _calendarService = calendarService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> List()
        {
            return View(await _eventService.ListAsync());
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Event model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await _eventService.CreateAsync(model);
            if(result == false)
            {
                ModelState.AddModelError("", "Błąd tworzenia wydarzenia.");
                return View(model);
            }
            return RedirectToAction("List");
        }

        
        public async Task<IActionResult> Remove (int id)
        {
            await _eventService.RemoveAsync(id);
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> Edit (int id)
        {
            return View(await _eventService.GetAsync(id));
        }
        [HttpPost]
        public async Task<IActionResult> Edit (Event model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await _eventService.UpdateAsync(model);
            if(result == false)
            {
                ModelState.AddModelError("", "Błąd aktualizacji wydarzenia.");
                return View(model);
            }
            return RedirectToAction("List");
        }

        
        public async Task<IActionResult> Details(int id)
        {
            return View(await _eventService.GetAsync(id));
        }

        public async Task<IActionResult> Calendar(DateTime dateTime, int id)
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
                Calendar = await _calendarService.GetReservationAsync(dateTime),
                EventId = id
            };
            return View(model);
        }
    }
}
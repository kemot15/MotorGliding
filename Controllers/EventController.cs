using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotorGliding.Models.Db;
using MotorGliding.Models.Enums;
using MotorGliding.Models.Other;
using MotorGliding.Models.ViewModels;
using MotorGliding.Services;
using MotorGliding.Services.Interfaces;

namespace MotorGliding.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly ICalendarService _calendarService;
        private readonly IImageService _imageService;

        public EventController(IEventService eventService, ICalendarService calendarService, IImageService imageService)
        {
            _eventService = eventService;
            _calendarService = calendarService;
            _imageService = imageService;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            //ViewData["Title"] = Tabs.Other;
            return RedirectToAction("List");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> List()
        {
            //ViewData["Title"] = Tabs.Other;
            return View(await _eventService.ListAsync());
        }

        //[HttpGet]
        //public IActionResult Add()
        //{
        //    return View();
        //}
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id = 0)
        {
            if (id == 0)
            return View();
            var item = await _eventService.GetAsync(id);
            if (item == null)
            {
                return View();
            }
            var image = await _imageService.GetMainAsync(item.Id, EventCategory.Event.ToString());
            if (image != null)
                item.Image = image;
            return View(item);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddEdit (Event model)
        {
            if (!ModelState.IsValid)
                return View(model);
            int result;
            if (model.Id == 0)
            {
                result = await _eventService.CreateAsync(model);
            } 
            else
            {
                result = await _eventService.UpdateAsync(model);
            }

            if (result == 0)
            {
                ModelState.AddModelError("", "Błąd wydarzenia.");
                return View(model);
            }
            if (model.Image !=null && model.Image.ImageFile != null)
            {
                var image = await _imageService.GetAsync(model.Image.Id);
                if (image != null)
                {
                    if (image.Name == model.Image.ImageFile.FileName)
                    {
                        return RedirectToAction("List");
                    }
                    else
                    {
                        await _imageService.DeleteImageAsync(image, Folders.images.ToString());
                        image = await _imageService.AddImageAsync(model.Image, Folders.images.ToString(), true);
                        await _imageService.UpdateImageAsync(image);
                        return RedirectToAction("List");
                    }
                }                
                model.Image.SourceId = result;
                model.Image.Category = EventCategory.Event.ToString();
                image = await _imageService.AddImageAsync(model.Image, Folders.images.ToString(), true);
                await _imageService.SaveImageAsync(image);
            }
                       
           
            return RedirectToAction("List");
        }
        //[HttpPost]
        //public async Task<IActionResult> Add(Event model)
        //{
        //    if (!ModelState.IsValid)
        //        return View(model);
        //    var result = await _eventService.CreateAsync(model);
        //    if (result == 0)
        //    {
        //        ModelState.AddModelError("", "Błąd tworzenia wydarzenia.");
        //        return View(model);
        //    }
        //    if (model.Image != null)
        //    {
        //        model.Image.SourceId = result;
        //        model.Image.Category = "Event";
        //        var image = await _imageService.AddImageAsync(model.Image, "images", true);
        //        await _imageService.SaveImageAsync(image);
        //    }            
        //    return RedirectToAction("List");
        //}

        
        /// <summary>
        /// Usuniecie Eventu
        /// </summary>
        /// <param name="id">Id Eventu do usuniecia</param>
        /// <returns>Przekierowanie do listy Eventow</returns>
        [Authorize (Roles = "Admin")]
        public async Task<IActionResult> Remove (int id)
        {
            var item = await _eventService.GetAsync(id);
            if (item == null)
                return RedirectToAction("List");
            var eventId = item.Id;
            var gallery = await _imageService.GetGalleryAsync(eventId, EventCategory.Event.ToString());
            if (gallery != null)
                foreach(var image in gallery)
                {            
                    await _imageService.DeleteImageAsync(image, Folders.images.ToString());
                    await _imageService.RemoveImageAsync(image);
                }
            await _eventService.RemoveAsync(item);
            return RedirectToAction("List");
        }

        //[HttpGet]
        //public async Task<IActionResult> Edit (int id)
        //{
        //    var item = await _eventService.GetAsync(id);
        //    item.Image = await _imageService.GetMainAsync(item.Id, "Event");
        //    return View(item);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Edit (Event model)
        //{
        //    if (!ModelState.IsValid)
        //        return View(model);
        //    var result = await _eventService.UpdateAsync(model);
        //    if(result == false)
        //    {
        //        ModelState.AddModelError("", "Błąd aktualizacji wydarzenia.");
        //        return View(model);
        //    }
        //    return RedirectToAction("List");
        //}

        /// <summary>
        /// Pobiera szczegoly dla danego Eventu
        /// </summary>
        /// <param name="id">Id Eventu do pobrania</param>
        /// <returns>Zwraca Event</returns>
        public async Task<IActionResult> Details(int id)
        {
            ViewBag.Tab = Tabs.Other;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MotorGliding.Models.ViewModels;
using MotorGliding.Services.Interfaces;

namespace MotorGliding.Controllers.Home
{
    public class HomeController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly IEventService _eventService;
        private readonly IImageService _imageService;

        public HomeController(IVehicleService vehicleService, IEventService eventService, IImageService imageService)
        {
            _vehicleService = vehicleService;
            _eventService = eventService;
            _imageService = imageService;
        }

        public async Task<ActionResult> Index()
        {
            var events = await _eventService.ListAsync();

            foreach(var item in events)
            {
                item.Image = await _imageService.GetMainAsync(item.Id, "Event");
            }
            
            var model = new MainPageViewModel()
            {
                Vehicle = await _vehicleService.GetMainAsync(),
                Events = events.Where(e => e.Visible == true).ToList()                
            };
            //model.Vehicle = await _vehicleService.GetMainAsync();
            //ViewBag.Image = $"{model.Vehicle.Images.First().Name}";           
            return View(model);
        }
    }
}
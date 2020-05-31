using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MotorGliding.Models.ViewModels;
using MotorGliding.Services.Email;
using MotorGliding.Services.Interfaces;
using Nancy.Json;

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

        public async Task<IActionResult> Index()
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

        public async Task<IActionResult> SendEmail([FromBody]EmailViewModel email)
        {
           if (email != null && !string.IsNullOrEmpty(email.Name) && !string.IsNullOrWhiteSpace(email.Email) && email.Phone != 0 && !string.IsNullOrWhiteSpace(email.Message))
            {
                email.IsHtml = true;
                email.Subject = "Wiadomość ze strony SkyClub - formularz kontaktowy";
                email.Body = $"<h1>Od: {email.Name}</h1>{Environment.NewLine}<h2>E-mail: {email.Email} </h2>{Environment.NewLine}<div>Wiadomość: {email.Message}</div>";

                await EmailService.SendEmailAsync(email);
                //info = "Wiadomość została wysłana";
                return Json(200);
            }
            //info = "Wiadomość nie została wysłana";
            return Json(400);

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MotorGliding.Models.ViewModels;
using MotorGliding.Services;
using MotorGliding.Services.Interfaces;

namespace MotorGliding.Controllers.Home
{
    public class HomeController : Controller
    {
        private readonly IVehicleService _vehicleService;

        public HomeController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }
        public async Task<ActionResult> Index()
        {
            var model = new MainPageViewModel();
            model.Vehicle = await _vehicleService.GetMainAsync();
            //ViewBag.Image = $"{model.Vehicle.Images.First().Name}";
            return View(model);
        }
    }
}
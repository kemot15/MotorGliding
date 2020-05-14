using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MotorGliding.Models.Db;
using MotorGliding.Services.Interfaces;

namespace MotorGliding.Controllers
{
    public class VehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> List()
        {
            return View(await _vehicleService.ListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Vehicle vehicle)
        {
            if (ModelState.IsValid)
            await _vehicleService.AddAsync(vehicle);
            return RedirectToAction("List");
        }
    }
}
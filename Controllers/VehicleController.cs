using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MotorGliding.Models.Db;
using MotorGliding.Services.Interfaces;
using SQLitePCL;

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
        public IActionResult Add()
        {           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Vehicle vehicle)
        {
            //if (!ModelState.IsValid)
            //    return View(vehicle);
            await _vehicleService.AddAsync(vehicle);
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            return View(await _vehicleService.GetAsync(id)); 
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Vehicle vehicle)
        {
            await _vehicleService.UpdateAsync(vehicle);
            return RedirectToAction("List");
        }

        public async Task<IActionResult> Remove(int id)
        {
            await _vehicleService.RemoveAsync(id);
            return RedirectToAction("List");
        }

    }
}
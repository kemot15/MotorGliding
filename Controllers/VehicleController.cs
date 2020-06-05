
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> List()
        {
            return View(await _vehicleService.ListAsync());
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Add()
        {           
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(Vehicle vehicle)
        {
            //if (!ModelState.IsValid)
            //    return View(vehicle);
            await _vehicleService.AddAsync(vehicle);
            return RedirectToAction("List");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            return View(await _vehicleService.GetAsync(id)); 
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(Vehicle vehicle)
        {
            await _vehicleService.UpdateAsync(vehicle);
            return RedirectToAction("List");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove(int id)
        {
            await _vehicleService.RemoveAsync(id);
            return RedirectToAction("List");
        }

    }
}
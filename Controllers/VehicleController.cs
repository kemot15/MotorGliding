
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotorGliding.Models.Db;
using MotorGliding.Models.Other;
using MotorGliding.Services.Interfaces;
namespace MotorGliding.Controllers
{
    public class VehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly IImageService _imageService;

        public VehicleController(IVehicleService vehicleService, IImageService imageService)
        {
            _vehicleService = vehicleService;
            this._imageService = imageService;
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

        [Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id = 0)
        {
            if (id == 0)
                return View();
            var item = await _vehicleService.GetAsync(id);
            if (item == null)
            {
                return View();
            }
            var image = await _imageService.GetMainAsync(item.Id, EventCategory.Vehicle.ToString());
            if (image != null)
                item.Image = image;
            return View(item);
        }
        [HttpPost]
        public async Task<IActionResult> AddEdit (Vehicle model)
        {
            if (!ModelState.IsValid)
                return View(model);
            int result;
            if (model.Id == 0)
            {
                result = await _vehicleService.AddAsync(model);
            }
            else
            {
                result = await _vehicleService.UpdateAsync(model);
            }

            if (result == 0)
            {
                ModelState.AddModelError("", "Błąd wydarzenia.");
                return View(model);
            }
            if (model.Image != null && model.Image.ImageFile != null)
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

    }
}
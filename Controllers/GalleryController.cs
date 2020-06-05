using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MotorGliding.Models.Db;
using MotorGliding.Models.Enums;
using MotorGliding.Models.Other;
using MotorGliding.Services.Interfaces;

namespace MotorGliding.Controllers
{
    public class GalleryController : Controller
    {
        private readonly IImageService _imageService;

        public GalleryController(IImageService imageService)
        {
            _imageService = imageService;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> List()
        {
            ViewBag.Active = Tabs.Gallery;
            return View(await _imageService.GetGalleryAsync(false));
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            ViewBag.Active = Tabs.Gallery;
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(Image model)
        {
            
            foreach(var image in model.Gallery)
            {
                var img = new Image
                {
                    Active = true,
                    Category = Folders.gallery.ToString(),
                    ImageFile = image,
                    Name = image.Name,
                    Default = false
                };
                await _imageService.AddImageAsync(img, Folders.gallery.ToString(), false);
                await _imageService.SaveImageAsync(img);
            }
            return RedirectToAction("List");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActiveSwitch (int id)
        {
            await _imageService.ActiveChangeAsync(id);
            return RedirectToAction("List");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete (int id)
        {
            var image = await _imageService.GetAsync(id);
            await _imageService.DeleteImageAsync(image, Folders.gallery.ToString());
            await _imageService.RemoveImageAsync(image);
            return RedirectToAction("List");
        }
    }
}


using System.Collections.Generic;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using MotorGliding.Models.Db;
using MotorGliding.Models.Enums;
using MotorGliding.Models.Other;
using MotorGliding.Models.ViewModels;
using MotorGliding.Services.Interfaces;
using SQLitePCL;

namespace MotorGliding.Controllers
{
    public class GalleryController : Controller
    {
        private readonly IImageService _imageService;

        public GalleryController(IImageService imageService)
        {
            _imageService = imageService;
        }

        public async Task<IActionResult> List()
        {
            ViewBag.Active = Tabs.Gallery;
            return View(await _imageService.GetGalleryAsync(false));
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            ViewBag.Active = Tabs.Gallery;
            return View();
        }

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

        public IActionResult ActiveSwich (int id)
        {
            _imageService.ActiveChange(id);
            return RedirectToAction("List");
        }

        public async Task<IActionResult> Delete (int id)
        {
            var image = await _imageService.GetAsync(id);
            await _imageService.DeleteImageAsync(image, Folders.gallery.ToString());
            await _imageService.RemoveImageAsync(image);
            return RedirectToAction("List");
        }
    }
}

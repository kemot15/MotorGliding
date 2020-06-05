using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MotorGliding.Services.Interfaces;
using Rotativa.AspNetCore;

namespace MotorGliding.Controllers
{
    public class ReportsController : Controller
    {

        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IOrderService _orderService;

        public ReportsController(IWebHostEnvironment hostEnvironment, IOrderService orderService)
        {
            _hostEnvironment = hostEnvironment;
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> OrderConfirmation(int id)
        {
            var order = await _orderService.GetPreviewAsync(id);
            return View(order);
        }

        public async Task<IActionResult> Preview(int id)
        {
            //var wwwRootPath = _hostEnvironment.WebRootPath;
            //string path = Path.Combine($"{wwwRootPath}/Reports/raport_{DateTime.Now}");
            var order = await _orderService.GetPreviewAsync(id);
            var result = new ViewAsPdf("OrderConfirmation", "Reports", order);
            //{
            //    FileName = (path)
            //};
            return result;
        }


        public string AttachmentGenerator()
        {
            var wwwRootPath = _hostEnvironment.WebRootPath;
            string path = Path.Combine($"{wwwRootPath}/Reports/raport_{DateTime.Now}");
            //var result = new ViewAsPdf("OrderConfirmation", "Reports")
            //{
            //    FileName = (path)
            //};
            return path;
        }
    }
}

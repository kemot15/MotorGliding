using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MotorGliding.Services.Interfaces;
using Rotativa.AspNetCore;

namespace MotorGliding.Controllers
{
    public class ReportsController : Controller
    {

        private readonly IOrderService _orderService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ReportsController(IOrderService orderService, IWebHostEnvironment webHostEnvironment)
        {
            _orderService = orderService;
            this._webHostEnvironment = webHostEnvironment;
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
            var wwwRootPath = _webHostEnvironment.WebRootPath;
            var path = $"{wwwRootPath}\\Reports\\{DateTime.Now.ToShortDateString()}_{id}.pdf";
            var order = await _orderService.GetPreviewAsync(id);
            var result = new ViewAsPdf("OrderConfirmation", "Reports", order)
            {
               // SaveOnServerPath = path                       //tworzy plik zamowienia na serwerze
            };
            return result;
        }
      
    }
}

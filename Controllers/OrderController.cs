using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MotorGliding.Models.Db;
using MotorGliding.Models.Enums;
using MotorGliding.Models.ViewModels;
using MotorGliding.Services.Email;
using MotorGliding.Services.Interfaces;
using Rotativa.AspNetCore;
using System.IO;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Server.IIS.Core;

namespace MotorGliding.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<User> UserManager;
        private readonly IAccountService _accountService;
        private readonly IEventService _eventService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public OrderController(IOrderService orderService, UserManager<User> userManager, IAccountService accountService, IEventService eventService, IWebHostEnvironment webHostEnvironment)
        {
            _orderService = orderService;
            UserManager = userManager;
            _accountService = accountService;
            this._eventService = eventService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Details()
        {
            ViewBag.Tab = Tabs.Other;
            //ViewBag.Tab = Tabs.Other;
            var cookie = Request.Cookies["orderID"];

            

            if (cookie != null)
            {
                if (!int.TryParse(Request.Cookies["orderId"], out int orderId))
                {
                    Response.Cookies.Delete("orderId");
                    return RedirectToAction("Error", "Home");
                }
                var order = await _orderService.GetAsync(orderId);
                if (order != null)
                {
                    var orderViewModel = new OrderWithDetailsViewModel
                    {
                        Order = order,
                        EventList = await _eventService.ListAsync()
                    };
                    return View(orderViewModel);
                }
                Response.Cookies.Delete("orderId");
            }
           // ViewBag.OrderId = orderId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Details(Event model)
        {
            // ViewData["Title"] = Tabs.Other;
            OrderWithDetailsViewModel orderViewModel;
            var cookie = Request.Cookies["orderID"];
            if (cookie == null)
            {
                var order = new Order
                {
                    OrderDetails = new List<OrderDetails>()
                };
                OrderDetails orderDetails = new OrderDetails
                {
                    Price = model.Price,
                    Quantity = model.Quantity,
                    EventID = model.Id,
                    //EventTitle = model.Title,
                    Camera = false,
                };
                //order.EventList = await _eventService.ListAsync();
                orderViewModel = new OrderWithDetailsViewModel
                {
                    Order = order,
                    EventList = await _eventService.ListAsync()

                };
                order.OrderDetails.Add(orderDetails);
                await _orderService.CreateAsync(order);
                Response.Cookies.Append("orderId", $"{order.Id}");
            }
            else
            {
                if (!int.TryParse(Request.Cookies["orderId"], out int orderId))
                {
                    Response.Cookies.Delete("orderId");
                    return RedirectToAction("Error", "Home");
                }
                var order = await _orderService.GetAsync(orderId);
                // var details = order.OrderDetails.SingleOrDefault();
                if (order == null)
                {
                    Response.Cookies.Delete("orderId");
                    return RedirectToAction("Details");
                }
                if (!order.OrderDetails.Any(d => d.EventID == model.Id))
                {
                    order.OrderDetails.Add(new OrderDetails
                    {
                        Price = model.Price,
                        CameraPrice = model.CameraPrice,
                        Quantity = model.Quantity,
                        EventID = model.Id,
                        //EventTitle = model.Title,
                        Camera = false,
                    });
                }
                else
                {
                    var orderDetails = order.OrderDetails.Single(d => d.EventID == model.Id);
                    orderDetails.Quantity+= model.Quantity;
                }
                await _orderService.UpdateAsync(order);
                orderViewModel = new OrderWithDetailsViewModel
                {
                    Order = order,
                    EventList = await _eventService.ListAsync()

                };               
                
            }
            //ViewData["Title"] = Tabs.Other;
            ViewBag.Tab = Tabs.Other;
            ViewBag.OrderId = cookie;
            return View(orderViewModel);
        }

        public async Task<IActionResult> Remove (int id)
        {
            await _orderService.RemoveAsync(id);
            return RedirectToAction("Details");
        }
        [HttpPost]
        public async Task<IActionResult> Refresh(List<OrderDetails> detail)
        {           
            await _orderService.UpdateOrderDetailsAsync(detail);
            return RedirectToAction("Details");
        }
      
        public async Task<IActionResult> RefreshPosition ([FromBody]List<OrderDetails> detail)
        {
             await _orderService.UpdateOrderDetailsAsync(detail);
            // return RedirectToAction("Details");
            return StatusCode(200);//Json(detail);
        }

        [HttpGet]
        public async Task<IActionResult> UserConfirm()
        {
            //ViewData["Title"] = Tabs.Other;
            ViewBag.Tab = Tabs.Other;
            var cookie = Request.Cookies["orderId"];

            if (cookie == null)            
                return RedirectToAction("Details");

            if (!int.TryParse(Request.Cookies["orderId"], out int orderId))
            {
                Response.Cookies.Delete("orderId");
                return RedirectToAction("Error", "Home");
            }

            var user = await UserManager.GetUserAsync(User);
            if (user != null)
            {
                var address = await _accountService.GetUserAddress(user.Id);
                if (address != null)
                    user.Address = address;
                else
                    user.Address = new Address();
                var model = new EditUserViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Street = user.Address.Street,
                    ZipCode = user.Address.ZipCode,
                    City = user.Address.City,
                    Country = user.Address.Country,
                    OrderId = orderId,
                    Email = user.Email

                };
                return View(model);
            }             
            return View(new EditUserViewModel { OrderId = orderId });
        }
        
        [HttpPost]
        public async Task<IActionResult> UserConfirm(EditUserViewModel model)
        {
            ViewBag.Tab = Tabs.Other;            

            if (model.OrderId <=0)
            {
                return RedirectToAction("Error", "Home");
            } 
            try { 
                var userOrderId = await _orderService.CreateUserAsync(model);
                await _orderService.OrderAccept(model.OrderId);
                await _orderService.UpdateOrderUserId(model.OrderId, userOrderId);
            }
            catch
            {
                Response.Cookies.Delete("orderId");
                return RedirectToAction("Error", "Home");
            }


            var wwwRootPath = _webHostEnvironment.WebRootPath;
            var path = $"{wwwRootPath}\\Reports\\{DateTime.Now.ToShortDateString()}_{model.OrderId}.pdf";
          

            //var result = new ViewAsPdf("OrderConfirmation", "Reports", model.OrderId)
            //{
            //    FileName = "test",
            //    SaveOnServerPath = path
            //};




            var userEmail = new EmailViewModel
            {
                From = null,
                Subject = "Potwierdzenie zamówienia ze strony SkyClub",
                IsHtml = true,
                Body = BodyHtmlGenerator(model.OrderId.ToString(), model.Name, model.LastName),
                //$"<h1>Potwierdzamy przyjęcie zamówienia</h1>{Environment.NewLine}<h2>{model.OrderId} {model.Name} {model.LastName}</h2>{Environment.NewLine}<div>Potwierdzamy otrzymanie zamówienia</div></br><a href=\"{context}\">Link do pobrania zamówienia</a>",
                To = model.Email,
               // PathAttachment = path,
            };

            var adminEmail = new EmailViewModel
            {
                From = model.Email,
                Subject = $"Potwierdzenie zamówienia ze strony SkyClub {model.OrderId}",
                IsHtml = true,
                Body = $"<h1>Nowe zamówienie</h1>{Environment.NewLine}<h2>Od: {model.OrderId} {model.Name} {model.LastName}</h2>{Environment.NewLine}<div>{model.PhoneNumber} {model.Street} {model.ZipCode} {model.City}</div>"
            };

            if (await EmailService.SendEmailAsync(userEmail))
            {
                await EmailService.SendEmailAsync(adminEmail);
                return RedirectToAction("OrderConfirm", "Order", new { id = model.OrderId }); //docelowo strona potwierdzenia      
                
            }
            return RedirectToAction("Error", "Home");
            
        }

        public IActionResult OrderConfirm(int id)
        {
            Response.Cookies.Delete("orderId");
            ViewBag.Tab = Tabs.Other;
            return View(id);
        }
        [Authorize]
        public async Task<IActionResult> OrderPreview(int id)
        {
            var user = await UserManager.GetUserAsync(User);
            var order = await _orderService.GetPreviewAsync(id);            
            if (order == null)
                return RedirectToAction("Index", "Dashboard");
            if (!order.Accepted)
                return RedirectToAction("Index", "Dashboard");
            if (user == null)
                return RedirectToAction("Index", "Dashboard");
            if (!await UserManager.IsInRoleAsync(user, "Admin"))
            if (user.Id != order.OrderUser.UserId)
                return RedirectToAction("Index", "Dashboard");
            var model = new OrderWithDetailsViewModel
            {
                Order = order,
                EventList = await _eventService.ListAsync()
        };
            return View(model);
        }


        //generuje zamowienie w formacie pdf na serwerze, zwraca sciezke do pliku
        public async Task<string> OrderConfirmationGeneratorAsync(int id)
        {
            var wwwRootPath = _webHostEnvironment.WebRootPath;
            var order = await _orderService.GetPreviewAsync(id);
            var path = $"{wwwRootPath}\\Reports\\{DateTime.Now.ToShortDateString()}_{id}.pdf";
            var result = new ViewAsPdf("OrderConfirmation", "Reports", order) 
            { 
                FileName = "test",
                SaveOnServerPath = path
            };
            result.ExecuteResult(ControllerContext);

           // var bytefile = result.BuildFile();
            //await result.ExecuteResultAsync(this.ControllerContext);
            //result.SaveOnServerPath = path;
            return  result.SaveOnServerPath;
        }       


        public async Task<IActionResult> Preview(int id, string path)
        {            
            var order = await _orderService.GetPreviewAsync(id);
            var result = new ViewAsPdf("OrderConfirmation", "Reports", order)
            {
                FileName = "test",
            };            
            var test = result.SaveOnServerPath = path;
            return result;
        }

        public string BodyHtmlGenerator(string orderId, string Name, string LastName)
        {
            var context = $"{HttpContext.Request.Host}/Reports/Preview/{orderId}";
            var wwwRootPath = _webHostEnvironment.WebRootPath;
            var path = $"{wwwRootPath}\\Reports\\EmailBodyOrderConfirmation.html";
            var html = System.IO.File.ReadAllText(path);
            html = html.Replace("{{ orderId }}", orderId);
            html = html.Replace("{{ Name }}", Name);
            html = html.Replace("{{ LastName }}", LastName);
            html = html.Replace("{{ context }}", context);
            return html;
        }

    }
}
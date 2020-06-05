using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using MotorGliding.Models.Db;
using MotorGliding.Models.Enums;
using MotorGliding.Models.ViewModels;
using MotorGliding.Services.Email;
using MotorGliding.Services.Interfaces;

namespace MotorGliding.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<User> UserManager;
        private readonly IAccountService _accountService;
        private readonly IEventService _eventService;

        public OrderController(IOrderService orderService, UserManager<User> userManager, IAccountService accountService, IEventService eventService)
        {
            _orderService = orderService;
            UserManager = userManager;
            _accountService = accountService;
            this._eventService = eventService;
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

            Uri baseUri = new Uri("localhost:44386/");
            Uri myUri = new Uri(baseUri, $"Reports/Previe?id={model.Id}");
            //var link = \"\\Reports\\Preview?id={ model.OrderId }\";

            var userEmail = new EmailViewModel
            {
                From = null,
                Subject = "Potwierdzenie zamówienia ze strony SkyClub",
                IsHtml = true,
                Body = $"<h1>Potwierdzamy przyjęcie zamówienia</h1>{Environment.NewLine}<h2>{model.OrderId} {model.Name} {model.LastName}</h2>{Environment.NewLine}<div>Potwierdzamy otrzymanie zamówienia</div></br><a href=\"www.wp.pl\">Link do pobrania zamówienia</a>",
                To = model.Email
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
                
            }
            return RedirectToAction("OrderConfirm", "Order", new { id = model.OrderId }); //docelowo strona potwierdzenia      
            
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
    }
}
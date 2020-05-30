using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MotorGliding.Models.Db;
using MotorGliding.Models.Other;
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

        public OrderController(IOrderService orderService, UserManager<User> userManager, IAccountService accountService)
        {
            _orderService = orderService;
            UserManager = userManager;
            _accountService = accountService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Details()
        {
            ViewData["Title"] = Tabs.Other.ToString();
            var cookie = Request.Cookies["orderID"];
            if (cookie != null)
            {
                var order = await _orderService.GetAsync(int.Parse(cookie));
                return View(order);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Details(Event model)
        {
            ViewData["Title"] = Tabs.Other.ToString();
            Order order;
            var cookie = Request.Cookies["orderID"];
            if (cookie == null)
            {
                order = new Order
                {
                    OrderDetails = new List<OrderDetails>()
                };
                OrderDetails orderDetails = new OrderDetails
                {
                    Price = model.Price,
                    Quantity = model.Quantity,
                    EventID = model.Id,
                    EventTitle = model.Title,
                    Camera = false,
                };
                order.OrderDetails.Add(orderDetails);
                await _orderService.CreateAsync(order);
                Response.Cookies.Append("orderId", $"{order.Id}");
            }
            else
            {
                order = await _orderService.GetAsync(int.Parse(cookie));
                
                if (!order.OrderDetails.Any(d => d.EventID == model.Id))
                {
                    order.OrderDetails.Add(new OrderDetails
                    {
                        Price = model.Price,
                        CameraPrice = model.CameraPrice,
                        Quantity = model.Quantity,
                        EventID = model.Id,
                        EventTitle = model.Title,
                        Camera = false,
                    });
                }
                else
                {
                    var orderDetails = order.OrderDetails.Single(d => d.EventID == model.Id);
                    orderDetails.Quantity+= model.Quantity;
                    orderDetails.EventTitle = model.Title;
                }
                await _orderService.UpdateAsync(order);
            } 
            return View(order);
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
            ViewData["Title"] = Tabs.Other;
            var orderId = Request.Cookies["orderId"];
            if (orderId == null)
                return RedirectToAction("Details");
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
                    OrderId = int.Parse(orderId),
                    Email = user.Email

                };
                
                return View(model);
            }
            
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> UserConfirm(EditUserViewModel model)
        {
            ViewData["Title"] = Tabs.Other;
            var orderId = int.Parse(Request.Cookies["orderId"]);
            model.OrderId = orderId;
            var userOrderId = await _orderService.CreateUserAsync(model);
            await _orderService.OrderAccept(orderId);
            await _orderService.UpdateOrderUserId(orderId, userOrderId);
            if (await EmailService.SendEmailAsync(null))
                Response.Cookies.Delete("orderId");
            return RedirectToAction("OrderConfirm", "Order"); //docelowo strona potwierdzenia      
            
        }

        public async Task<IActionResult> OrderConfirm()
        {
            //var cookie = int.Parse(Request.Cookies["orderID"]);
            ViewData["Title"] = Tabs.Other;
            return View();
        }

        public async Task<IActionResult> OrderPreview(int id)
        {
            var order = await _orderService.GetPreviewAsync(id);
            if (order == null)
                RedirectToAction("Index", "Dashboard");
            return View(order);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MotorGliding.Models.Db;
using MotorGliding.Services.Interfaces;

namespace MotorGliding.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<User> UserManager;

        public OrderController(IOrderService orderService, UserManager<User> userManager)
        {
            _orderService = orderService;
            UserManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Details()
        {
            var cookie = Request.Cookies["orderID"];
            if (cookie != null)
            {
                var order = await _orderService.GetAsync(int.Parse(cookie));
                return View(order);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Details(Event model, int quantity = 1)
        {
            Order order;
            var cookie = Request.Cookies["orderID"];            
            if (cookie == null)
            {
                order = new Order();
                order.OrderDetails = new List<OrderDetails>();
                OrderDetails orderDetails = new OrderDetails
                {
                    Price = model.Price,
                    Quantity = quantity,
                    EventID = model.Id,
                 //   Event = model,
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
                        Quantity = quantity,
                        EventID = model.Id,
                       // Event = model,
                        Camera = false,
                    });
                }
                else
                {
                    var orderDetails = order.OrderDetails.Single(d => d.EventID == model.Id);
                  //  orderDetails.Event = model;
                    orderDetails.Quantity++;
                    //orderDetails.Price = model.Price * orderDetails.Quantity;
                }
                //order.OrderDetails.Add(orderDetails);
                await _orderService.UpdateAsync(order);
             //   order.OrderDetails
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

        [HttpGet]
        public async Task<IActionResult> UserConfirm()
        {
            if (Request.Cookies["orderId"] == null)
                return RedirectToAction("Details");
            var user = await UserManager.GetUserAsync(User);            
            if (user != null)
            {
                return View(user);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UserConfirm(User user, Address address)
        {
            var userId = (user.Id == 0 ? await _orderService.CreateUserAsync(user) : await _orderService.UpdateUserAsync(user));

            //    var userId = await _orderService.CreateUserAsync(user);


            var orderId = int.Parse(Request.Cookies["orderId"]);
            await _orderService.UpdateOrderUserId(orderId, userId);
            return RedirectToAction("Index", "Home");
        }
    }
}
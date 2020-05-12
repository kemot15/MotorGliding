using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MotorGliding.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Active = "Dashboard";
            return View();
        }

        public IActionResult Account()
        {
            ViewBag.Active = "Account";
            return View();
        }
    }
}
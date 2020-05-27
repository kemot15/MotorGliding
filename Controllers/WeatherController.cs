using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MotorGliding.Models.Other;
using Nancy.Json;

namespace MotorGliding.Controllers
{
    public class WeatherController : Controller
    {
        public IActionResult Index()
        {       
            string appid = "3da830a4d8a131e854ec8f7b49f61132";
            string city = "Warsaw";
            string url = string.Format($"https://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&appid={appid}");
            var client = new WebClient();
            var json = client.DownloadString(url);
            var result = (new JavaScriptSerializer()).Deserialize<WeatherInfo.Root>(json);
            WeatherInfo.Root outPut = result;
            return View(outPut);
        }

    }
}
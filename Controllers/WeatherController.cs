using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MotorGliding.Models.Enums;
using MotorGliding.Models.Other;
using Nancy.Json;
using RestSharp;

namespace MotorGliding.Controllers
{
    public class WeatherController : Controller
    {
        public IActionResult Index(string city ="Wroclaw")
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json", true);
            var config = configurationBuilder.Build();
            
            string appid = config.GetSection("WeatherConfig:Appid").Value;//"3da830a4d8a131e854ec8f7b49f61132";
           // string city = "Warsaw";
            string url = string.Format($"https://api.openweathermap.org/data/2.5/forecast?q={city}&units=metric&appid={appid}");
            var client = new WebClient();
            var json = client.DownloadString(url);
            //var result = (new JavaScriptSerializer()).Deserialize<WeatherInfo.Root>(json);
            //WeatherInfo.Root outPut = result;
            var result = (new JavaScriptSerializer()).Deserialize<ForecastWeather.Rootobject>(json);
            ForecastWeather.Rootobject outPut = result;
            ViewData["Title"] = Tabs.Other.ToString();


            //var client1 = new RestClient("https://api.openweathermap.org/data/2.5/forecast?q={city}&units=metric");
            //var request = new RestRequest(Method.GET);
            ////request.AddHeader("x-rapidapi-host", "weather2020-weather-v1.p.rapidapi.com");
            //request.AddHeader("x-rapidapi-key", "3da830a4d8a131e854ec8f7b49f61132");
            //IRestResponse response = client1.Execute(request);

            return View(outPut);
        }
    }
}
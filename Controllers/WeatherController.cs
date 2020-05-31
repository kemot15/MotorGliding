using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MotorGliding.Models.Enums;
using Nancy.Json;
using static MotorGliding.Models.Enums.ForecastWeather;

namespace MotorGliding.Controllers
{
    public class WeatherController : Controller
    {
        public IActionResult Index(string city ="Wroclaw")
        {
            ViewBag.Tab = Tabs.Other;
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json", true);
            var config = configurationBuilder.Build();
            
            string appid = config.GetSection("WeatherConfig:Appid").Value;//"3da830a4d8a131e854ec8f7b49f61132";
           // string city = "Warsaw";
            string url = string.Format($"https://api.openweathermap.org/data/2.5/forecast?q={city}&units=metric&lang=pl&appid={appid}");
            var json = "";
            var client = new WebClient();
            try
            {
                json = client.DownloadString(url);
            }
            catch(Exception)
            {
                ViewBag.Info = "Miejscowość nie została odnaleziona";
                return Index("Wrocław");
            }
            
            //if (json == null)
            //{
            //    ViewBag.Info = "Miejscowość nie została odnaleziona";
            //    Index();
            //}
            var result = (new JavaScriptSerializer()).Deserialize<ForecastWeather.Rootobject>(json);
            ForecastWeather.Rootobject rootobject = result;



            url = string.Format($"https://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&lang=pl&appid={appid}");
            client = new WebClient();
            json = client.DownloadString(url);
            var rootResult = (new JavaScriptSerializer()).Deserialize<ForecastWeather.Root>(json);
            ForecastWeather.Root root = rootResult;
            //var today = (new JavaScriptSerializer()).Deserialize<WeatherInfo.Root>(json);
            //WeatherInfo.Root outPutToday = today;
            var forecast = new Forecast()
            {
                rootobject = rootobject,
                root = root
            };

            //ViewData["Title"] = Tabs.Other;
            //ViewBag.Tab = Tabs.Other;


            //var client1 = new RestClient("https://api.openweathermap.org/data/2.5/forecast?q={city}&units=metric");
            //var request = new RestRequest(Method.GET);
            ////request.AddHeader("x-rapidapi-host", "weather2020-weather-v1.p.rapidapi.com");
            //request.AddHeader("x-rapidapi-key", "3da830a4d8a131e854ec8f7b49f61132");
            //IRestResponse response = client1.Execute(request);

            return View(forecast);
        }
    }
}
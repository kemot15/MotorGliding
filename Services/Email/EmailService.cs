using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MotorGliding.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MotorGliding.Services.Email
{
    public class EmailService
    {
        public static async Task<bool> SendEmailAsync(EmailViewModel model)
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json", true);
            var config = configurationBuilder.Build();
            var emailFrom = config.GetSection("EmailConfig:EmailFrom").Value;
            var pass = config.GetSection("EmailConfig:Pass").Value;
            var emailTo = config.GetSection("EmailConfig:EmailTo").Value;
            var host = config.GetSection("EmailConfig:Host").Value;
            var port = config.GetSection("EmailConfig:Port").Value;
            MailMessage message = new MailMessage
            {
                //From = new MailAddress(emailFrom),//From = new MailAddress(model.From),
                //Subject = "Wiadomość wysłana ze strony SkyClub - potwierdzenie zamówienia",
                //Body = $"<h1>Od: {model.Name}</h1>{Environment.NewLine}<h2>E-mail: {model.Email}</h2>{Environment.NewLine}<div>Treść: {model.Message}</div>",
                //IsBodyHtml = true
                From = new MailAddress(model.Email),//From = new MailAddress(model.From),
                Subject = model.Subject,//"Wiadomość wysłana ze strony SkyClub - potwierdzenie zamówienia",
                Body = model.Body,//$"<h1>Od: strona zamowien</h1>{Environment.NewLine}<h2>E-mail: zee strony </h2>{Environment.NewLine}<div>Treść: potwierdzenie zamowienia</div>",
                IsBodyHtml = model.IsHtml

            };
            //message.To.Add(model.To);
            message.To.Add(emailTo);

            SmtpClient client = new SmtpClient
            {
                Credentials = new NetworkCredential(emailFrom, pass),
                Host = host,
                Port = int.Parse(port),
                EnableSsl = true,
                Timeout = 5000
            };
            await client.SendMailAsync(message);
            //var info = true;

            return true;
        }
    }
}

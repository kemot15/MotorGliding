using Microsoft.Extensions.Configuration;
using MotorGliding.Models.ViewModels;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MotorGliding.Services.Email
{
    public class EmailService
    {   
        public static async Task<bool> SendEmailAsync(EmailViewModel model)
        {
            try
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
                From = new MailAddress(model.Email == null ? emailFrom : model.Email),//From = new MailAddress(model.From),
                Subject = model.Subject,//"Wiadomość wysłana ze strony SkyClub - potwierdzenie zamówienia",
                Body = model.Body,//$"<h1>Od: strona zamowien</h1>{Environment.NewLine}<h2>E-mail: zee strony </h2>{Environment.NewLine}<div>Treść: potwierdzenie zamowienia</div>",
                IsBodyHtml = model.IsHtml,
                //Attachments = atachhment

            };
 
            message.To.Add(model.To == null ? emailTo : model.To);

            SmtpClient client = new SmtpClient
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(emailFrom, pass),
                Host = host,
                Port = int.Parse(port),
                EnableSsl = true,
                Timeout = 5000,
               
            };
            //await client.SendMailAsync(message);
           
                client.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            //var info = true;

          //  return false;
        }
    }
}

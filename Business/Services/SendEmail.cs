using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Business
{
    public class SendEmail : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {


            string EmailOrigen = "cadavidcamilo360@gmail.com";
            string clave = "pbczcevcfbxykbky";


            var message = new MailMessage(EmailOrigen, email)
            {
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };


            SmtpClient osmtpClient = new SmtpClient();
            osmtpClient.Host = "smtp.gmail.com";
            osmtpClient.EnableSsl = true;
            osmtpClient.UseDefaultCredentials = false;
            osmtpClient.Port = 587;
            osmtpClient.Credentials = new NetworkCredential(EmailOrigen, clave);

            try
            {

                osmtpClient.Send(message);
                osmtpClient.Dispose();

                return Task.CompletedTask;

            }
            catch (Exception ex)
            {

                throw new Exception("Error al enviar el correo electrónico ", ex);
            }

            
        }
    }
}

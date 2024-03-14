using System.Threading.Tasks;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Application.Infrastructure.Email
{
    public class EmailSender
    {
        private readonly IConfiguration _config;
        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        public bool SendEmailAsync(string userEmail, string emailSubject, string msg)
        {

            string fromMail = "filip.soljic@gmail.com";
            string fromPassword = "apkqwmczuxgtjled";
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("test@localhost");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = emailSubject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = msg;

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(fromMail, fromPassword);
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            //SmtpClient client = new SmtpClient();
            //client.DeliveryMethod = SmtpDeliveryMethod.
            // SpecifiedPickupDirectory;
            //client.PickupDirectoryLocation = @"C:\Test";


            //client.Send("test@localhost", userEmail,
            //       "Confirm your email",
            //   confirmationLink);

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);// log exception
            }
            return false;
        }
    }
}

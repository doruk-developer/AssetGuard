using AssetGuard.Business.Abstract;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace AssetGuard.Business.Concrete
{
    public class EmailManager : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var mailSettings = _configuration.GetSection("MailSettings");

            var host = mailSettings["Host"];
            var port = int.Parse(mailSettings["Port"]);
            var fromMail = mailSettings["Mail"];
            var displayName = mailSettings["DisplayName"];
            var username = mailSettings["Username"]; // Mailtrap Username'i buradan alır
            var password = mailSettings["Password"]; // Mailtrap Password'ü buradan alır

            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(fromMail, displayName);
            mailMessage.To.Add(to);
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;

            using (var client = new SmtpClient(host, port))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(username, password);
                client.EnableSsl = true;

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
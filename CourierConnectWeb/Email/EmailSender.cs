using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Net;
using System.Net.Mail;

namespace CourierConnectWeb.Email
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = "gina.grant@ethereal.email";
            var pw = "TuhBmP7t1FF3cM1NXb";

            var client = new SmtpClient("smtp.ethereal.email", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, pw)
            };

            return client.SendMailAsync(new MailMessage(
                from: mail,
                to: email,
                subject,
                message));
        }
    }
}

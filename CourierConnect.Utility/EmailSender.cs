using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.Utility
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string subject, string toEmail, string message)
        {
            var apiKey = "SG.Bq_jGT5IRvCa8QPBEewZQA.I3TQw9BC4pQEwfwZkp7ikViHs3L8BBk2g9iGqoEvAEA";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("wojtas.domis@gmail.com", "CourierConnect");
            var to = new EmailAddress(toEmail, toEmail);
            var plainTextContent = message;
            var htmlContent = "";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}

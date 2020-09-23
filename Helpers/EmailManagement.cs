using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace KariyerAppApi.Helpers
{
    public class EmailManagement : IEmailManagement
    {
        private readonly IConfiguration _config;

        public EmailManagement(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailForRegistiration(string newUserName, string newUserEmail)
        {
            var apiKey = _config.GetSection("SENDGRID_API_KEY").Value;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("ismailcanvardar@gmail.com", "Example User 1");
            List<EmailAddress> tos = new List<EmailAddress>
          {
              new EmailAddress(newUserEmail, "Example User 2"),
              //new EmailAddress("test3@example.com", "Example User 3"),
              //new EmailAddress("test4@example.com","Example User 4")
          };

            var subject = $"{newUserName} KariyerApp'e hoşgeldin.";
            var htmlContent = $"<strong>{newUserName} aramıza hoşgeldin.</strong>";
            var displayRecipients = false; // set this to true if you want recipients to see each others mail id 
            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, subject, "", htmlContent, displayRecipients);
            var response = await client.SendEmailAsync(msg);
        }

        public async Task SendEmailToEmployerAboutApplication(string advertName, string employerEmail)
        {
            var apiKey = _config.GetSection("SENDGRID_API_KEY").Value;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("ismailcanvardar@gmail.com", "Example User 1");
            List<EmailAddress> tos = new List<EmailAddress>
          {
              new EmailAddress(employerEmail, "Example User 2"),
              //new EmailAddress("test3@example.com", "Example User 3"),
              //new EmailAddress("test4@example.com","Example User 4")
          };

            var subject = "KariyerApp ilanınıza başvuru yapıldı.";
            var htmlContent = $"<strong>{advertName} başlıklı ilanınıza başvuru yapıldı.</strong>";
            var displayRecipients = false; // set this to true if you want recipients to see each others mail id 
            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, subject, "", htmlContent, displayRecipients);
            var response = await client.SendEmailAsync(msg);
        }

        public async Task<int> SendVerificationCode(string to)
        {
            Random rnd = new Random();
            int verificationCode = rnd.Next(100000, 999999);
            var apiKey = _config.GetSection("SENDGRID_API_KEY").Value;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("ismailcanvardar@gmail.com", "Example User 1");
            List<EmailAddress> tos = new List<EmailAddress>
          {
              new EmailAddress(to, "Example User 2"),
              //new EmailAddress("test3@example.com", "Example User 3"),
              //new EmailAddress("test4@example.com","Example User 4")
          };

            var subject = "KariyerApp Onay Kodu";
            var htmlContent = $"<strong>Onay kodunuz: {verificationCode}</strong>";
            var displayRecipients = false; // set this to true if you want recipients to see each others mail id 
            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, subject, "", htmlContent, displayRecipients);
            var response = await client.SendEmailAsync(msg);

            return verificationCode;
        }
    }
}

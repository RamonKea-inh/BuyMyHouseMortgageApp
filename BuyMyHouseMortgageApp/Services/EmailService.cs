using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BuyMyHouseMortgageApp.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendOfferEmailAsync(string recipientName, string offerDetails)
        {
            // Logic to send an email to the recipient. Maybe mocking the email service, because I don't have access to the email service and this is a proof of concept
            //var apiKey = _configuration["EmailServiceApiKey"];
            //var client = new SendGridClient(apiKey);
            //var from = new EmailAddress(_configuration["EmailServiceSender"], "Buy My House");
            //var subject = "Your Mortgage Offer";
            //var to = new EmailAddress(recipientName);
            //var plainTextContent = $"Dear {recipientName},\n\nHere are the details of your mortgage offer:\n\n{offerDetails}\n\nBest regards,\nBuy My House";
            //var htmlContent = $"<strong>Dear {recipientName},</strong><br><br>Here are the details of your mortgage offer:<br><br>{offerDetails}<br><br>Best regards,<br>Buy My House";
            //var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            //var response = await client.SendEmailAsync(msg);

            return Task.CompletedTask;
        }
    }
}

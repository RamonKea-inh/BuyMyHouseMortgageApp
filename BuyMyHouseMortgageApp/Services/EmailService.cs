using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BuyMyHouseMortgageApp.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendOfferEmailAsync(string recipientEmail, string offerDetails)
        {
            try
            {
                var apiKey = _configuration["EmailServiceApiKey"];
                if (string.IsNullOrEmpty(apiKey))
                {
                    throw new InvalidOperationException("SendGrid API key is not configured");
                }

                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(_configuration["EmailServiceSender"], "Buy My House");
                var to = new EmailAddress(recipientEmail);
                var subject = "Your Mortgage Offer";
                var plainTextContent = $"Dear {recipientEmail},\n\nHere are the details of your mortgage offer:\n\n{offerDetails}\n\nBest regards,\nBuy My House";
                var htmlContent = $@"
                    <div>
                        <p><strong>Dear {recipientEmail},</strong></p>
                        <p>Here are the details of your mortgage offer:</p>
                        <p>{offerDetails}</p>
                        <p>Best regards,<br/>Buy My House</p>
                    </div>";

                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to send email. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Log the exception here
                throw new Exception("Failed to send email offer", ex);
            }
        }
    }
}
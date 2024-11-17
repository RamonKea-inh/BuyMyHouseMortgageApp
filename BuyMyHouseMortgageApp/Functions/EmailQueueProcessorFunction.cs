using BuyMyHouseMortgageApp.Models;
using BuyMyHouseMortgageApp.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BuyMyHouseMortgageApp.Functions
{
    public class EmailQueueProcessorFunction
    {
        private readonly ILogger<EmailQueueProcessorFunction> _logger;
        private readonly IEmailService _emailService;

        public EmailQueueProcessorFunction(ILogger<EmailQueueProcessorFunction> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        [Function("EmailQueueProcessorFunction")]
        public async Task Run([QueueTrigger("emailqueue", Connection = "AzureWebJobsStorage")] string queueMessage)
        {
            _logger.LogInformation($"Processing email queue message: {queueMessage}");

            EmailMessage emailMessage;
            try
            {
                emailMessage = JsonConvert.DeserializeObject<EmailMessage>(queueMessage);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize queue message to EmailMessage.");
                return;
            }

            if (emailMessage == null)
            {
                _logger.LogError("Failed to deserialize queue message to EmailMessage.");
                return;
            }

            // Send the offer email to the applicant
            await _emailService.SendOfferEmailAsync(emailMessage.RecipientName, emailMessage.OfferDetails);

            _logger.LogInformation($"Offer email sent to: {emailMessage.RecipientName}");
        }
    }
}

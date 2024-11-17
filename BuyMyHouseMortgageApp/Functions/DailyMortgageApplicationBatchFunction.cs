using Azure.Storage.Queues;
using BuyMyHouseMortgageApp.Models;
using BuyMyHouseMortgageApp.Repositories;
using BuyMyHouseMortgageApp.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BuyMyHouseMortgageApp.Functions
{
    public class DailyMortgageApplicationBatchFunction
    {
        private readonly ILogger _logger;
        private readonly IMortgageApplicationRepository _mortgageApplicationRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly QueueClient _queueClient;

        public DailyMortgageApplicationBatchFunction(ILoggerFactory loggerFactory, IMortgageApplicationRepository mortgageApplicationRepository, IConfiguration configuration, IEmailService emailService)
        {
            _logger = loggerFactory.CreateLogger<DailyMortgageApplicationBatchFunction>();
            _mortgageApplicationRepository = mortgageApplicationRepository;
            _configuration = configuration;
            _emailService = emailService;
            _queueClient = new QueueClient(_configuration["AzureWebJobsStorage"], "emailqueue");
            _queueClient.CreateIfNotExists();
        }

        [Function("DailyMortgageApplicationBatchFunction")]
        public async Task Run([TimerTrigger("0 0 0 * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }

            try
            {
                var expiredApplications = await _mortgageApplicationRepository.GetExpiredMortgageApplicationsAsync();

                // Process the expired mortgage applications, e.g., generate and send offers
                foreach (var application in expiredApplications)
                {
                    // Process the expired application
                    await ProcessExpiredMortgageApplicationAsync(application);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing mortgage applications.");
                throw;
            }
        }

        private async Task ProcessExpiredMortgageApplicationAsync(MortgageApplication application)
        {
            // Generate the mortgage offer
            var offer = GenerateMortgageOffer(application);

            // Update the application status and offer details
            application.ApplicationStatus = "Offer Generated";
            application.OfferDetails = offer.ToString();
            application.OfferExpiration = DateTime.UtcNow.AddDays(7); // Offer valid for 7 days

            await _mortgageApplicationRepository.UpdateMortgageApplicationAsync(application);

            // Enqueue the email message
            var emailMessage = new EmailMessage
            {
                RecipientName = application.ApplicantName,
                OfferDetails = offer.ToString()
            };
            var messageJson = JsonConvert.SerializeObject(emailMessage);
            await _queueClient.SendMessageAsync(messageJson);

            _logger.LogInformation($"Offer email sent to: {application.ApplicantName}");
        }

        private MortgageOffer GenerateMortgageOffer(MortgageApplication application)
        {
            // Proof of concept logic to generate the mortgage offer based on the application details
            return new MortgageOffer
            {
                LoanAmount = application.LoanAmount,
                InterestRate = 4.5m,
                Term = 30
            };
        }
    }
}

using System;
using System.Configuration;
using BuyMyHouseMortgageApp.Models;
using BuyMyHouseMortgageApp.Repositories;
using BuyMyHouseMortgageApp.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BuyMyHouseMortgageApp.DailyMortgageApplicationBatch
{
    public class DailyMortgageApplicationBatchFunction
    {
        private readonly ILogger _logger;
        private readonly IMortgageApplicationRepository _mortgageApplicationRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public DailyMortgageApplicationBatchFunction(ILoggerFactory loggerFactory, IMortgageApplicationRepository mortgageApplicationRepository, IConfiguration configuration, IEmailService emailService)
        {
            _logger = loggerFactory.CreateLogger<DailyMortgageApplicationBatchFunction>();
            _mortgageApplicationRepository = mortgageApplicationRepository;
            _configuration = configuration;
            _emailService = emailService;
        }

        [Function("DailyMortgageApplicationBatchFunction")]
        public async Task DailyMortgageApplicationBatch([TimerTrigger("0 0 0 * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            
            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }

            var expiredApplications = await _mortgageApplicationRepository.GetExpiredMortgageApplicationsAsync();

            // Process the expired mortgage applications, e.g., generate and send offers
            foreach (var application in expiredApplications)
            {
                // Implement the logic to process the expired application
                await ProcessExpiredMortgageApplicationAsync(application);
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

            // Send the offer email to the applicant
            await _emailService.SendOfferEmailAsync(application.ApplicantName, offer.ToString());
        }

        private MortgageOffer GenerateMortgageOffer(MortgageApplication application)
        {
            // Implement the logic to generate the mortgage offer based on the application details
            return new MortgageOffer
            {
                LoanAmount = application.LoanAmount,
                InterestRate = 4.5m,
                Term = 30
            };
        }
    }
}

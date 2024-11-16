using BuyMyHouseMortgageApp.Models;
using BuyMyHouseMortgageApp.Repositories;
using BuyMyHouseMortgageApp.Services;

namespace BuyMyHouseMortgageApp.DailyMortgageApplicationBatch
{
    public class DailyMortgageApplicationBatch
    {
        private readonly IMortgageApplicationRepository _mortgageApplicationRepository;
        private readonly IEmailService _emailService;

        public DailyMortgageApplicationBatch(IMortgageApplicationRepository mortgageApplicationRepository, IEmailService emailService)
        {
            _mortgageApplicationRepository = mortgageApplicationRepository;
            _emailService = emailService;
        }

        public async Task ProcessExpiredApplicationsAsync()
        {
            var expiredApplications = await _mortgageApplicationRepository.GetExpiredMortgageApplicationsAsync();

            foreach (var application in expiredApplications)
            {
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

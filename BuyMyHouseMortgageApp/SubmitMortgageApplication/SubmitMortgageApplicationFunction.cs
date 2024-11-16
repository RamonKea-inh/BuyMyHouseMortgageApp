using BuyMyHouseMortgageApp.Models;
using BuyMyHouseMortgageApp.Repositories;
using BuyMyHouseMortgageApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace BuyMyHouseMortgageApp.SubmitMortgageApplication
{
    public class SubmitMortgageApplicationFunction
    {
        private readonly ILogger _logger;
        private readonly IHouseRepository _houseRepository;
        private readonly IMortgageApplicationRepository _mortgageApplicationRepository;
        private readonly IEmailService _emailService;

        public SubmitMortgageApplicationFunction(ILogger logger, IHouseRepository houseRepository, IMortgageApplicationRepository mortgageApplicationRepository, IEmailService emailService)
        {
            _logger = logger;
            _houseRepository = houseRepository;
            _mortgageApplicationRepository = mortgageApplicationRepository;
            _emailService = emailService;
        }

        [Function("SubmitMortgageApplication")]
        public async Task<IActionResult> SubmitMortgageApplication([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
        {
            _logger.LogInformation("Processing mortgage application submission request.");

            // Deserialize the request body
            var application = await req.ReadFromJsonAsync<MortgageApplication>();

            // Validate the input
            if (string.IsNullOrEmpty(application?.ApplicantName) || application.ApplicantIncome <= 0 || application.LoanAmount <= 0 || application.PropertyId <= 0)
            {
                _logger.LogWarning("Invalid mortgage application input: {application}", application);
                return new BadRequestObjectResult("Invalid mortgage application data.");
            }

            // Set default application properties
            application.ApplicationStatus = "Pending";
            application.OfferExpiration = DateTime.UtcNow.AddDays(30);
            application.OfferDetails = "Pending review";

            try
            {
                // Save the mortgage application
                await _mortgageApplicationRepository.CreateMortgageApplicationAsync(application);

                _logger.LogInformation("Mortgage application submitted successfully: {application}", application);
                return new OkObjectResult("Mortgage application submitted successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting mortgage application: {application}", application);
                return new ObjectResult("Error submitting mortgage application.") { StatusCode = 500 };
            }
        }
    }
}

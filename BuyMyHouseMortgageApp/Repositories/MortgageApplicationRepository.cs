using Azure.Data.Tables;
using BuyMyHouseMortgageApp.Models;
using Microsoft.Extensions.Configuration;

namespace BuyMyHouseMortgageApp.Repositories
{
    public class MortgageApplicationRepository : IMortgageApplicationRepository
    {
        private readonly TableClient _mortgageTableClient;
        private readonly TableClient _houseTableClient;
        private const string MortgageTableName = "MortgageApplications";
        private const string HouseTableName = "Houses";

        public MortgageApplicationRepository(IConfiguration configuration)
        {
            var connectionString = configuration["AzureStorage:ConnectionString"];
            var tableServiceClient = new TableServiceClient(connectionString);

            // Create tables if they don't exist
            _mortgageTableClient = tableServiceClient.GetTableClient(MortgageTableName);
            _mortgageTableClient.CreateIfNotExists();

            _houseTableClient = tableServiceClient.GetTableClient(HouseTableName);
            _houseTableClient.CreateIfNotExists();
        }

        public async Task CreateMortgageApplicationAsync(MortgageApplication application)
        {
            var entity = new MortgageApplicationEntity
            {
                PartitionKey = application.PropertyId.ToString(), // Using PropertyId as partition key
                RowKey = Guid.NewGuid().ToString(),
                Id = application.Id,
                ApplicantName = application.ApplicantName,
                ApplicantIncome = application.ApplicantIncome,
                LoanAmount = application.LoanAmount,
                PropertyId = application.PropertyId,
                ApplicationStatus = application.ApplicationStatus,
                OfferExpiration = application.OfferExpiration,
                OfferDetails = application.OfferDetails
            };

            await _mortgageTableClient.AddEntityAsync(entity);
        }

        public async Task<MortgageApplication> GetMortgageApplicationByIdAsync(int applicationId)
        {
            try
            {
                // Query all partitions for the specific application ID
                var query = _mortgageTableClient.QueryAsync<MortgageApplicationEntity>(
                    filter: $"ApplicationId eq {applicationId}");

                await foreach (var entity in query)
                {
                    return new MortgageApplication
                    {
                        Id = entity.Id,
                        ApplicantName = entity.ApplicantName,
                        ApplicantIncome = entity.ApplicantIncome,
                        LoanAmount = entity.LoanAmount,
                        PropertyId = entity.PropertyId,
                        ApplicationStatus = entity.ApplicationStatus,
                        OfferExpiration = entity.OfferExpiration,
                        OfferDetails = entity.OfferDetails
                    };
                }

                return null;
            }
            catch (Azure.RequestFailedException)
            {
                return null;
            }
        }

        public async Task<IEnumerable<MortgageApplication>> GetExpiredMortgageApplicationsAsync()
        {
            var expiryDate = DateTime.UtcNow.AddDays(-30); // Define expiry as 30 days
            var query = _mortgageTableClient.QueryAsync<MortgageApplicationEntity>(
                filter: $"ApplicationDate lt {expiryDate:o}"); // Filter by applications older than expiry date

            var expiredApplications = new List<MortgageApplication>();

            await foreach (var entity in query)
            {
                expiredApplications.Add(new MortgageApplication
                {
                    Id = entity.Id,
                    ApplicantName = entity.ApplicantName,
                    ApplicantIncome = entity.ApplicantIncome,
                    LoanAmount = entity.LoanAmount,
                    PropertyId = entity.PropertyId,
                    ApplicationStatus = entity.ApplicationStatus,
                    OfferExpiration = entity.OfferExpiration,
                    OfferDetails = entity.OfferDetails
                });
            }

            return expiredApplications;
        }

        public async Task UpdateMortgageApplicationAsync(MortgageApplication application)
        {
            var entity = new MortgageApplicationEntity
            {
                PartitionKey = application.PropertyId.ToString(),
                RowKey = application.Id.ToString(),
                Id = application.Id,
                ApplicantName = application.ApplicantName,
                ApplicantIncome = application.ApplicantIncome,
                LoanAmount = application.LoanAmount,
                PropertyId = application.PropertyId,
                ApplicationStatus = application.ApplicationStatus,
                OfferExpiration = application.OfferExpiration,
                OfferDetails = application.OfferDetails,
                ETag = Azure.ETag.All // Update the entity regardless of the ETag
            };

            await _mortgageTableClient.UpdateEntityAsync(entity, entity.ETag);
        }
    }
}

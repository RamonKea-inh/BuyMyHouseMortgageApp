using Azure.Data.Tables;

namespace BuyMyHouseMortgageApp.Models
{
    public class MortgageApplicationEntity : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public Azure.ETag ETag { get; set; }

        // MortgageApplication properties
        public int Id { get; set; }
        public string ApplicantName { get; set; }
        public decimal ApplicantIncome { get; set; }
        public decimal LoanAmount { get; set; }
        public int PropertyId { get; set; }
        public string ApplicationStatus { get; set; }
        public DateTime OfferExpiration { get; set; }
        public string OfferDetails { get; set; }
    }

}

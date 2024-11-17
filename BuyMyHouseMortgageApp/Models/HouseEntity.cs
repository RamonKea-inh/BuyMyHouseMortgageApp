using Azure.Data.Tables;

namespace BuyMyHouseMortgageApp.Models
{
    public class HouseEntity : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public Azure.ETag ETag { get; set; }

        public int Id { get; set; }
        public string Address { get; set; }
        public float Price { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public float SquareMeters { get; set; }
        public string ImageURL { get; set; }
    }
}

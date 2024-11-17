using Azure.Data.Tables;
using BuyMyHouseMortgageApp.Models;
using BuyMyHouseMortgageApp.Services;
using Microsoft.Extensions.Configuration;

namespace BuyMyHouseMortgageApp.Repositories
{
    public class HouseRepository : IHouseRepository
    {
        private readonly TableClient _tableClient;
        private const string TableName = "Houses";
        private readonly IBlobStorageService _blobStorageService;

        public HouseRepository(IConfiguration configuration, IBlobStorageService blobStorageService)
        {
            var connectionString = configuration["AzureStorage:ConnectionString"];
            var tableServiceClient = new TableServiceClient(connectionString);
            _tableClient = tableServiceClient.GetTableClient(TableName);
            _tableClient.CreateIfNotExists();

            _blobStorageService = blobStorageService;
        }

        public async Task CreateHouseAsync(House house, string imagePath)
        {
            // Upload image to blob storage and get the URL
            var imageUrl = await _blobStorageService.UploadImageAsync(imagePath, $"{house.Id}.jpg");

            var entity = new HouseEntity
            {
                PartitionKey = "House",
                RowKey = house.Id.ToString(),
                Id = house.Id,
                Address = house.Address,
                Price = house.Price,
                Bedrooms = house.Bedrooms,
                Bathrooms = house.Bathrooms,
                SquareMeters = house.SquareMeters,
                ImageURL = house.ImageURL
            };

            await _tableClient.AddEntityAsync(entity);
        }

        public async Task<House> GetHouseByIdAsync(int houseId)
        {
            try
            {
                var response = await _tableClient.GetEntityAsync<HouseEntity>("House", houseId.ToString());
                var entity = response.Value;

                return new House
                {
                    Id = entity.Id,
                    Address = entity.Address,
                    Price = entity.Price,
                    Bedrooms = entity.Bedrooms,
                    Bathrooms = entity.Bathrooms,
                    SquareMeters = entity.SquareMeters,
                    ImageURL = entity.ImageURL
                };
            }
            catch (Azure.RequestFailedException)
            {
                return null;
            }
        }

        public async Task<IEnumerable<House>> GetHousesAsync()
        {
            var query = _tableClient.QueryAsync<HouseEntity>(filter: $"PartitionKey eq 'House'");
            var houses = new List<House>();

            await foreach (var entity in query)
            {
                houses.Add(new House
                {
                    Id = entity.Id,
                    Address = entity.Address,
                    Price = entity.Price,
                    Bedrooms = entity.Bedrooms,
                    Bathrooms = entity.Bathrooms,
                    SquareMeters = entity.SquareMeters,
                    ImageURL = entity.ImageURL
                });
            }

            return houses;
        }

        public async Task<IEnumerable<House>> SearchHousesByPriceRangeAsync(float minPrice, float maxPrice)
        {
            var query = _tableClient.QueryAsync<HouseEntity>(
                filter: $"PartitionKey eq 'House' and Price ge {minPrice} and Price le {maxPrice}");
            var houses = new List<House>();

            await foreach (var entity in query)
            {
                houses.Add(new House
                {
                    Id = entity.Id,
                    Address = entity.Address,
                    Price = entity.Price,
                    Bedrooms = entity.Bedrooms,
                    Bathrooms = entity.Bathrooms,
                    SquareMeters = entity.SquareMeters,
                    ImageURL = entity.ImageURL
                });
            }

            return houses;
        }
    }
}

using BuyMyHouseMortgageApp.Models;
using BuyMyHouseMortgageApp.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace BuyMyHouseMortgageApp.Functions
{
    public class HouseFunction
    {
        private readonly IHouseService _houseService;
        private readonly ILogger _logger;

        public HouseFunction(IHouseService houseService, ILoggerFactory loggerFactory)
        {
            _houseService = houseService;
            _logger = loggerFactory.CreateLogger<HouseFunction>();
        }

        [Function("AddHouseWithImage")]
        public async Task<HttpResponseData> AddHouseWithImageAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "houses")] HttpRequestData req,
            FunctionContext executionContext)
        {
            _logger.LogInformation("Processing request to add a new house with image.");

            // Read the request body
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<AddHouseRequest>(requestBody);

            // Validate the request
            if (data == null || data.House == null || string.IsNullOrEmpty(data.ImagePath))
            {
                var badRequestResponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                await badRequestResponse.WriteStringAsync("Invalid request payload.");
                return badRequestResponse;
            }

            // Check if the image file exists
            if (!File.Exists(data.ImagePath))
            {
                var notFoundResponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
                await notFoundResponse.WriteStringAsync("Image file not found.");
                return notFoundResponse;
            }

            try
            {
                // Add the house with the image
                await _houseService.CreateHouse(data.House, data.ImagePath);

                var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
                await response.WriteStringAsync("House added successfully.");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the house with image.");
                var errorResponse = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                await errorResponse.WriteStringAsync("An error occurred while processing your request.");
                return errorResponse;
            }
        }

        [Function("GetHouses")]
        public async Task<HttpResponseData> GetHousesAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "houses")] HttpRequestData req,
            FunctionContext executionContext)
        {
            _logger.LogInformation("Processing request to get all houses.");

            var houses = await _houseService.GetHouses();
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(houses);
            return response;
        }

        [Function("GetHouseById")]
        public async Task<HttpResponseData> GetHouseByIdAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "houses/{id:int}")] HttpRequestData req,
            int id,
            FunctionContext executionContext)
        {
            _logger.LogInformation($"Processing request to get house with ID: {id}");

            var house = await _houseService.GetHouseById(id);
            if (house == null)
            {
                var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
                await notFoundResponse.WriteStringAsync("House not found.");
                return notFoundResponse;
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(house);
            return response;
        }

        [Function("SearchHousesByPriceRange")]
        public async Task<HttpResponseData> SearchHousesByPriceRangeAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "houses/search")] HttpRequestData req,
            FunctionContext executionContext)
        {
            _logger.LogInformation("Processing request to search houses by price range.");

            var queryParams = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            if (!float.TryParse(queryParams["minPrice"], out var minPrice) || !float.TryParse(queryParams["maxPrice"], out var maxPrice))
            {
                var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badRequestResponse.WriteStringAsync("Invalid price range parameters.");
                return badRequestResponse;
            }

            var houses = await _houseService.SearchHousesByPriceRange(minPrice, maxPrice);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(houses);
            return response;
        }
    }
}

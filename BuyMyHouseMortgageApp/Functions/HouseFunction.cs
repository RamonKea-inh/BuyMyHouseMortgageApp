using BuyMyHouseMortgageApp.Models;
using BuyMyHouseMortgageApp.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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

            // Add the house with the image
            await _houseService.CreateHouse(data.House, data.ImagePath);

            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteStringAsync("House added successfully.");
            return response;
        }
    }
}

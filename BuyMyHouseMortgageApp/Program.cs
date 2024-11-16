using BuyMyHouseMortgageApp.Repositories;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddScoped<IMortgageApplicationRepository, MortgageApplicationRepository>();
        services.AddScoped<IHouseRepository, HouseRepository>();

    })
    .Build();

host.Run();
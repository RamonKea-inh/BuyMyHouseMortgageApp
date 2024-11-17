using BuyMyHouseMortgageApp.Repositories;
using BuyMyHouseMortgageApp.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration(configBuilder =>
    {
        configBuilder
            .SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    })
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.AddLogging();
        services.ConfigureFunctionsApplicationInsights();

        // Register services
        services.AddScoped<IMortgageApplicationRepository, MortgageApplicationRepository>();
        services.AddScoped<IHouseRepository, HouseRepository>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IMortgageApplicationService, MortgageApplicationService>();
        services.AddScoped<IHouseService, HouseService>();

        // Register BlobStorageService with connection string from configuration
        services.AddScoped<IBlobStorageService>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var connectionString = configuration["AzureStorage:ConnectionString"];
            return new BlobStorageService(connectionString);
        });
    })
    .Build();

host.Run();
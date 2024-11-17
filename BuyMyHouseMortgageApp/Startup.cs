using BuyMyHouseMortgageApp.Repositories;
using BuyMyHouseMortgageApp.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuyMyHouseMortgageApp
{
    public class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            var context = builder.GetContext();
            builder.ConfigurationBuilder
                .SetBasePath(context.ApplicationRootPath)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddLogging();

            //builder.Services.AddSingleton<IDbConnectionFactory, SqlDbConnectionFactory>();
            builder.Services.AddScoped<IMortgageApplicationRepository, MortgageApplicationRepository>();
            builder.Services.AddScoped<IHouseRepository, HouseRepository>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IMortgageApplicationService, MortgageApplicationService>();
            builder.Services.AddScoped<IHouseService, HouseService>();
        }
    }
}

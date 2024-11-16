using BuyMyHouseMortgageApp.DailyMortgageApplicationBatch;
using BuyMyHouseMortgageApp.Repositories;
using BuyMyHouseMortgageApp.SubmitMortgageApplication;
using Microsoft.AspNetCore.Connections;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            //builder.Services.AddSingleton<IDbConnectionFactory, SqlDbConnectionFactory>();
            builder.Services.AddScoped<IMortgageApplicationRepository, SubmitMortgageApplicationRepository>();
            builder.Services.AddScoped<IMortgageApplicationRepository, DailyMortgageApplicationBatchRepository>();
            //builder.Services.AddScoped<IHouseRepository, HouseRepository>();
        }
    }
}

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using BuyMyHouse.BLL.Interfaces;
using BuyMyHouse.BLL;
using BuyMyHouse.DAL.Repository;
using BuyMyHouse.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using BuyMyHouse.DAL;
using Microsoft.Extensions.Configuration;
using System.IO;

[assembly: FunctionsStartup(typeof(House.API.Startup))]
namespace House.API
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<IHouseService, HouseService>();
            builder.Services.AddScoped<IHouseRepository, HouseRepository>();

            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("local.settings.json", true, true)
            .Build();

            builder.Services.AddDbContext<BuyMyHouseContext>(options =>
                options.UseSqlServer(configuration["SqlConnectionString"]));
        }
    }
}
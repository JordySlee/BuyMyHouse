using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.EntityFrameworkCore;
using BuyMyHouse.DAL;
using BuyMyHouse.BLL.Interfaces;
using BuyMyHouse.DAL.Repository;
using BuyMyHouse.DAL.Interfaces;
using BuyMyHouse.BLL;

[assembly: FunctionsStartup(typeof(User.API.Startup))]
namespace User.API
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("local.settings.json", true, true)
            .Build();


            builder.Services.AddDbContext<BuyMyHouseContext>(options =>
                options.UseSqlServer(configuration["SqlConnectionString"]));            
        }
    }
}


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BuyMyHouse.DAL
{
    public class BuyMyHouseContextFactory : IDesignTimeDbContextFactory<BuyMyHouseContext>
    { 
        public BuyMyHouseContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", true, true)
                .Build();

            string connection = configuration["SqlConnectionString"];
            var optionsBuilder = new DbContextOptionsBuilder<BuyMyHouseContext>();

            optionsBuilder.UseSqlServer(connection);

            return new BuyMyHouseContext(optionsBuilder.Options);
        }
    }
}


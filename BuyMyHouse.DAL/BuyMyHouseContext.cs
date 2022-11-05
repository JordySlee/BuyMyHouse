using BuyMyHouse.Model.Entity;
using Microsoft.EntityFrameworkCore;



namespace BuyMyHouse.DAL
{
    public class BuyMyHouseContext : DbContext
    {
        public DbSet<House> Houses { get; set; }
        public DbSet<User> Users { get; set; }

        public BuyMyHouseContext(DbContextOptions<BuyMyHouseContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}


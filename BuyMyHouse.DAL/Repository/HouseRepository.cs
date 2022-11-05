using BuyMyHouse.DAL.Interfaces;
using BuyMyHouse.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace BuyMyHouse.DAL.Repository
{
    public class HouseRepository : IHouseRepository
    {
        private readonly BuyMyHouseContext _buyMyHouseContext;
        public HouseRepository(BuyMyHouseContext buyMyHouseContext)
        {
            _buyMyHouseContext = buyMyHouseContext;
        }

        public async Task CreateHouse(House house)
        {
            await _buyMyHouseContext.Houses.AddAsync(house);
            await _buyMyHouseContext.SaveChangesAsync();
        }

        public async Task<List<House>> GetAllHouses()
        {
            return await _buyMyHouseContext.Houses.ToListAsync();
        }

        public async Task<List<House>> GetAllHousesInPriceRange(double lowerBound, double upperbound)
        {
            return await _buyMyHouseContext.Houses
                .Where(h => h.Price >= lowerBound && h.Price <= upperbound)
                .ToListAsync();
        }

        public async Task<House?> GetHouse(Guid houseId)
        {
            try
            {
                return await _buyMyHouseContext.Houses.FindAsync(houseId);

            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task UpdateHouse(House newHouse)
        {
            try
            {
                House? oldHouse = await _buyMyHouseContext.Houses.FindAsync(newHouse.HouseId);

                if (oldHouse != null && oldHouse.SoldAt == null)
                {
                    _buyMyHouseContext.Entry(oldHouse).CurrentValues.SetValues(newHouse);
                    await _buyMyHouseContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                return;
            }

        }

        public async Task DeleteHouse(Guid houseId)
        {
            try
            {
                House? oldHouse = await _buyMyHouseContext.Houses.FindAsync(houseId);

                if (oldHouse != null && oldHouse.SoldAt == null)
                {
                    House newHouse = oldHouse;
                    newHouse.SoldAt = DateTime.Now;
                    _buyMyHouseContext.Entry(oldHouse).CurrentValues.SetValues(newHouse);
                    await _buyMyHouseContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                return;
            }
        }
    }
}

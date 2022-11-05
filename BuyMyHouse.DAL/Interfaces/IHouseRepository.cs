using BuyMyHouse.Model.Entity;

namespace BuyMyHouse.DAL.Interfaces
{
    public interface IHouseRepository
    {
        Task<List<House>> GetAllHouses();
        Task<List<House>> GetAllHousesInPriceRange(double lowerBound, double upperbound);
        Task<House?> GetHouse(Guid houseId);
        Task CreateHouse(House house);
        Task UpdateHouse(House house);
        Task DeleteHouse(Guid houseId);
    }
}

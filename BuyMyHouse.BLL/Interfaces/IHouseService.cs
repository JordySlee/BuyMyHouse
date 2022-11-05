using BuyMyHouse.Model.Entity;

namespace BuyMyHouse.BLL.Interfaces
{
    public interface IHouseService
    {
        Task<House> GetHouse(Guid houseId);
        Task<List<House>> GetAllHouses();
        Task<List<House>> GetAllHousesInPriceRange(double lowerBound, double upperBound);
        Task<House> CreateHouse(string name);
        Task UpdateHouse(string name, Guid houseId);
        Task DeleteHouse(Guid houseId);
    }
}

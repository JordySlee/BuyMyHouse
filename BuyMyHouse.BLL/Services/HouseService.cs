using BuyMyHouse.BLL.Interfaces;
using BuyMyHouse.Model.Entity;
using BuyMyHouse.DAL.Interfaces;

namespace BuyMyHouse.BLL
{
    public class HouseService : IHouseService
    {
        private IHouseRepository _houseRepository;

        public HouseService(IHouseRepository houseRepository)
        {
            _houseRepository = houseRepository;
        }

        public async Task<List<House>> GetAllHouses()
        {
            return await _houseRepository.GetAllHouses();
        }
        public async Task<List<House>> GetAllHousesInPriceRange(double lowerBound, double upperbound)
        {
            return await _houseRepository.GetAllHousesInPriceRange(lowerBound, upperbound);
        }

        public async Task<House> GetHouse(Guid houseId)
        {
            return await _houseRepository.GetHouse(houseId);
        }

        public async Task<House> CreateHouse(string name)
        {
            House house = new();
            house.Name = name;
            house.HouseId = Guid.NewGuid();
            await _houseRepository.CreateHouse(house);

            return house;
        }

        public async Task UpdateHouse(string name, Guid houseId)
        {
            House house = new();
            house.Name = name;
            house.HouseId = houseId;
            await _houseRepository.UpdateHouse(house);
        }

        public async Task DeleteHouse(Guid houseId)
        {
            await _houseRepository.DeleteHouse(houseId);
        }

    }
}

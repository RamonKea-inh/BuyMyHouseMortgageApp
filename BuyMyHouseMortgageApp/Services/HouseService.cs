using BuyMyHouseMortgageApp.Models;
using BuyMyHouseMortgageApp.Repositories;

namespace BuyMyHouseMortgageApp.Services
{
    public class HouseService : IHouseService
    {
        private readonly IHouseRepository _houseRepository;

        public HouseService(IHouseRepository houseRepository)
        {
            _houseRepository = houseRepository;
        }

        public Task<House> GetHouseById(int houseId)
        {
            return _houseRepository.GetHouseByIdAsync(houseId);
        }

        public async Task<IEnumerable<House>> GetHouses()
        {
            return await _houseRepository.GetHousesAsync();
        }

        public Task CreateHouse(House house, string imagePath)
        {
            return _houseRepository.CreateHouseAsync(house, imagePath);
        }

        public async Task<IEnumerable<House>> SearchHousesByPriceRange(float minPrice, float maxPrice)
        {
            return await _houseRepository.SearchHousesByPriceRangeAsync(minPrice, maxPrice);
        }
    }
}

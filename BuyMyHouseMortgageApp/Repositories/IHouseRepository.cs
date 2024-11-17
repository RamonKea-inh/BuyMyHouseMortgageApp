using BuyMyHouseMortgageApp.Models;

namespace BuyMyHouseMortgageApp.Repositories
{
    public interface IHouseRepository
    {
        Task<House> GetHouseByIdAsync(int houseId);
        Task<IEnumerable<House>> GetHousesAsync();
        Task CreateHouseAsync(House house, string imagePath);
        Task<IEnumerable<House>> SearchHousesByPriceRangeAsync(float minPrice, float maxPrice);
    }
}

using BuyMyHouseMortgageApp.Models;

namespace BuyMyHouseMortgageApp.Services
{
    public interface IHouseService
    {
        Task<IEnumerable<House>> GetHouses();

        Task<House> GetHouseById(int houseId);

        Task CreateHouse(House house, string imagePath);
    }
}

using BuyMyHouseMortgageApp.Models;
using BuyMyHouseMortgageApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}

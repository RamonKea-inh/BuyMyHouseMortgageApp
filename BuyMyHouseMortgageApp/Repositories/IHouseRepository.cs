using BuyMyHouseMortgageApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyMyHouseMortgageApp.Repositories
{
    public interface IHouseRepository
    {
        Task<House> GetHouseByIdAsync(int houseId);
        Task<IEnumerable<House>> GetHousesAsync();
        //Task CreateHouseAsync(House house);
    }
}

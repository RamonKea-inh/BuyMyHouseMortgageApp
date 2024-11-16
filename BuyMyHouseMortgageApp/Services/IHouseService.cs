using BuyMyHouseMortgageApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyMyHouseMortgageApp.Services
{
    public interface IHouseService
    {
        Task<IEnumerable<House>> GetHouses();

        Task<House> GetHouseById(int houseId);
    }
}

using BuyMyHouseMortgageApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyMyHouseMortgageApp.Repositories
{
    public class HouseRepository : IHouseRepository
    {
        //public Task CreateHouseAsync(House house)
        //{
        //    throw new NotImplementedException();
        //}

        public Task<House> GetHouseByIdAsync(int houseId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<House>> GetHousesAsync()
        {
            throw new NotImplementedException();
        }
    }
}

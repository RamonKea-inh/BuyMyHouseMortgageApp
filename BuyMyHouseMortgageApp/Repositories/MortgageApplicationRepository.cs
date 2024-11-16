using BuyMyHouseMortgageApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyMyHouseMortgageApp.Repositories
{
    public class MortgageApplicationRepository : IMortgageApplicationRepository
    {
        public Task CreateMortgageApplicationAsync(MortgageApplication application)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MortgageApplication>> GetExpiredMortgageApplicationsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<MortgageApplication> GetMortgageApplicationByIdAsync(int applicationId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateMortgageApplicationAsync(MortgageApplication application)
        {
            throw new NotImplementedException();
        }
    }
}

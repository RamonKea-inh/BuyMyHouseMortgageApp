using BuyMyHouseMortgageApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyMyHouseMortgageApp.Repositories
{
    public interface IMortgageApplicationRepository
    {
        Task CreateMortgageApplicationAsync(MortgageApplication application);
        Task<MortgageApplication> GetMortgageApplicationByIdAsync(int applicationId);
        Task UpdateMortgageApplicationAsync(MortgageApplication application);
        Task<IEnumerable<MortgageApplication>> GetExpiredMortgageApplicationsAsync();
    }
}

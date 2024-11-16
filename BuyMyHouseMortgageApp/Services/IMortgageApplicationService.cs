using BuyMyHouseMortgageApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyMyHouseMortgageApp.Services
{
    public interface IMortgageApplicationService
    {
        Task<MortgageApplication> GetMortgageApplicationByIdAsync(int applicationId);
        Task<IEnumerable<MortgageApplication>> GetExpiredMortgageApplicationsAsync();
        Task CreateMortgageApplicationAsync(MortgageApplication application);
        Task UpdateMortgageApplicationAsync(MortgageApplication application);
    }
}

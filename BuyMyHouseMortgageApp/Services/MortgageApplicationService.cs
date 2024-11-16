using BuyMyHouseMortgageApp.Models;
using BuyMyHouseMortgageApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyMyHouseMortgageApp.Services
{
    public class MortgageApplicationService : IMortgageApplicationService
    {
        private readonly IMortgageApplicationRepository _mortgageApplicationRepository;

        public MortgageApplicationService(IMortgageApplicationRepository mortgageApplicationRepository)
        {
            _mortgageApplicationRepository = mortgageApplicationRepository;
        }

        public async Task<MortgageApplication> GetMortgageApplicationByIdAsync(int applicationId)
        {
            return await _mortgageApplicationRepository.GetMortgageApplicationByIdAsync(applicationId);
        }

        public async Task<IEnumerable<MortgageApplication>> GetExpiredMortgageApplicationsAsync()
        {
            return await _mortgageApplicationRepository.GetExpiredMortgageApplicationsAsync();
        }

        public async Task CreateMortgageApplicationAsync(MortgageApplication application)
        {
            await _mortgageApplicationRepository.CreateMortgageApplicationAsync(application);
        }

        public async Task UpdateMortgageApplicationAsync(MortgageApplication application)
        {
            await _mortgageApplicationRepository.UpdateMortgageApplicationAsync(application);
        }
    }
}

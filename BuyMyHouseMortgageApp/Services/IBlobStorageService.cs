using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyMyHouseMortgageApp.Services
{
    public interface IBlobStorageService
    {
        Task<string> UploadImageAsync(string filePath, string fileName);
    }
}

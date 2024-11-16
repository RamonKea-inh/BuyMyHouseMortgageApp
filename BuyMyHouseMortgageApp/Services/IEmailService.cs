using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyMyHouseMortgageApp.Services
{
    public interface IEmailService
    {
        Task SendOfferEmailAsync(string recipientName, string offerDetails);
    }
}

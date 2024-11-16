using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyMyHouseMortgageApp.Models
{
    public class MortgageApplication
    {
        public int Id { get; set; }
        public string ApplicantName { get; set; } = string.Empty;
        public decimal ApplicantIncome { get; set; }
        public decimal LoanAmount { get; set; }
        public int PropertyId { get; set; }
        public string ApplicationStatus { get; set; } = string.Empty;
        public DateTime OfferExpiration { get; set; }
        public string OfferDetails { get; set; } = string.Empty;
    }
}

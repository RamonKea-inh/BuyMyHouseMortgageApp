using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyMyHouseMortgageApp.Models
{
    public class MortgageOffer
    {
        public decimal LoanAmount { get; set; }
        public decimal InterestRate { get; set; }
        public int Term { get; set; }

        public override string ToString()
        {
            return $"Loan Amount: {LoanAmount:C}, Interest Rate: {InterestRate:P2}, Term: {Term} years";
        }
    }
}

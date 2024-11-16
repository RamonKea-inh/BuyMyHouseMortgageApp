using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyMyHouseMortgageApp.Models
{
    public class House
    {
        public int Id { get; set; }
        public string Address { get; set; } = string.Empty;
        public float Price { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public float SquareFeet { get; set; }
    }
}

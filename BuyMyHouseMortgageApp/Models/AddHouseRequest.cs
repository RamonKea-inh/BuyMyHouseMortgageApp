using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyMyHouseMortgageApp.Models
{
    public class AddHouseRequest
    {
        public required House House { get; set; }
        public required string ImagePath { get; set; }
    }
}

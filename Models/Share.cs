using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCBTest.Models
{
    public class Share
    {
        public int Num_Shares { get; set; }
        public decimal Share_Price { get; set; }
        public decimal balace { get; set; }
        public decimal CalculBalance()
        {
            return Num_Shares * Share_Price;
        }
    }

}
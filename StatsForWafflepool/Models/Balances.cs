using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StatsForWafflepool.Models
{
    public class Balances
    {
        public int Id { get; set; }
        public double sent { get; set; }
        public double confirmed { get; set; }
        public double unconverted { get; set; }
    }
}
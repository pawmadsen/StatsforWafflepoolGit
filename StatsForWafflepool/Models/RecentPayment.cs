using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace StatsForWafflepool.Models
{
    public class RecentPayment
    {
        public int Id { get; set; }
        public double amount { get; set; }
        public DateTime time { get; set; }
        public string txn { get; set; }
    }
}
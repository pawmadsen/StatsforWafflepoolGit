using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StatsForWafflepool.Models
{

    public class RootObject
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public double hash_rate { get; set; }
        public virtual Balances balances { get; set; }
        public string error { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<RecentPayment> recent_payments { get; set; }
    }

}
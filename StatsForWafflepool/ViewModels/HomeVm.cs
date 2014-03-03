using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using StatsForWafflepool.Models;

namespace StatsForWafflepool.ViewModels
{
    public class HomeVm
    {
        
        public DotNet.Highcharts.Highcharts Chart1 { get; set; }
        public DotNet.Highcharts.Highcharts Chart2 { get; set; }
        public DotNet.Highcharts.Highcharts Chart3 { get; set; }
        public DotNet.Highcharts.Highcharts Chart4 { get; set; }
        public ApplicationUser User { get; set; }
        public int TotalUsers { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N3}")]
        public double CurrentHash { get; set; }
                [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N3}")]
        public double  AvghashFourHours { get; set; }
                [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N3}")]
        public double AvghashTwentyFourHours { get; set; }
                [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N6}")]
        public double AvgEarningsPerDay { get; set; }
                [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N3}")]
        public double AvghashAlltime { get; set; }
                [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N6}")]
        public double BcPerMh { get; set; }
        public ICollection<RecentPayment> RecentPayments { get; set; }
    }
}
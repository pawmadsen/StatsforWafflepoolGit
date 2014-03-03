using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using StatsForWafflepool.Models;
using StatsForWafflepool.ViewModels;
using Highcharts = DotNet.Highcharts.Highcharts;

namespace StatsForWafflepool.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var yesterday = DateTime.UtcNow.AddDays(-1);
            var userId = User.Identity.GetUserId();
            var data = new List<RootObject>();
            var model = new HomeVm();
            model.TotalUsers = db.Users.Count();



            if (userId != null)
            {
                
                
                data = db.RootObjects.Where(x => x.User.Id == userId).ToList();
                model.User = db.Users.Find(userId);
                model.RecentPayments = data.ToList().LastOrDefault().recent_payments;
                model.CurrentHash = data.ToList().LastOrDefault().hash_rate/1000000;

                model.AvghashFourHours =
                    data.Where(x => x.DateTime > DateTime.UtcNow.AddHours(-4)).Average(x => x.hash_rate)/1000000;

                model.AvghashTwentyFourHours =
                            data.Where(x => x.DateTime > DateTime.UtcNow.AddDays(-1)).Average(x => x.hash_rate) / 1000000;

                var distinctDays = model.RecentPayments.DistinctBy(x => x.time.Date);
                var payouts = new List<double>();
                var recentPayouts = new List<AverageBcPerMh>();
                var bcPerMh = new List<AverageBcPerMh>();
                foreach (var day in distinctDays)
                {
                    var amount = model.RecentPayments.Where(x => x.time.Date == day.time.Date).Sum(x => x.amount);
                    payouts.Add(amount);
                    recentPayouts.Add(new AverageBcPerMh()
                    {
                        Amount = amount,
                        DateTime = day.time.Date
                    });
                    var ro = data.Where(x => x.DateTime.Date.Equals(day.time.Date)).ToList();
                    if (!ro.Any()) continue;
                    var avgMh = ro.Average(x => x.hash_rate);
                    avgMh = avgMh / 1000000;
                    bcPerMh.Add(new AverageBcPerMh()
                    {
                        Amount = amount / avgMh,
                        DateTime = day.time.Date

                    });
                }
                data = data.Where(x => x.DateTime > yesterday).ToList();
                model.AvgEarningsPerDay = payouts.Average();

                model.AvghashAlltime = db.RootObjects.Where(x => x.User.Id == userId).Average(x => x.hash_rate)/1000000;
                model.BcPerMh = model.AvgEarningsPerDay/model.AvghashAlltime;


                model.Chart1 = new DotNet.Highcharts.Highcharts("chart1")
                    .InitChart(new Chart {DefaultSeriesType = ChartTypes.Spline})
                    .SetOptions(new GlobalOptions {Global = new Global {UseUTC = false}})
                    .SetTitle(new Title {Text = "Current MH/s"})
                    .SetXAxis(new XAxis
                    {
                        Type = AxisTypes.Datetime,

                    })
                    .SetYAxis(new YAxis
                    {
                        Title = new YAxisTitle {Text = "MH/s"},

                    })

                    .SetSeries(new[]
                    {
                        new Series
                        {
                            Name = "MH/s",
                            Data =
                                new Data(
                                    data.Select(
                                        x =>
                                            new Point()
                                            {
                                                X = Tools.GetTotalMilliseconds(x.DateTime),
                                                Y = x.hash_rate/1000000
                                            }).ToArray())
                        }
                    });
                model.Chart2 = new DotNet.Highcharts.Highcharts("chart2")
                    .InitChart(new Chart {DefaultSeriesType = ChartTypes.Spline})
                    .SetOptions(new GlobalOptions {Global = new Global {UseUTC = false}})
                    .SetTitle(new Title {Text = "Balances"})
                    .SetXAxis(new XAxis
                    {
                        Type = AxisTypes.Datetime,

                    })
                    .SetYAxis(new YAxis
                    {
                        Title = new YAxisTitle {Text = "Bitcoins"},

                    })

                    .SetSeries(new[]
                    {
                        new Series
                        {
                            Name = "Confirmed",
                            Data =
                                new Data(
                                    data.Select(
                                        x =>
                                            new Point()
                                            {
                                                X = Tools.GetTotalMilliseconds(x.DateTime),
                                                Y = x.balances.confirmed
                                            }).ToArray())
                        },
                        new Series
                        {
                            Name = "Unconverted",
                            Data =
                                new Data(
                                    data.Select(
                                        x =>
                                            new Point()
                                            {
                                                X = Tools.GetTotalMilliseconds(x.DateTime),
                                                Y = x.balances.unconverted
                                            }).ToArray())
                        },
                        new Series
                        {
                            Name = "Combined",
                            Data =
                                new Data(
                                    data.Select(
                                        x =>
                                            new Point()
                                            {
                                                X = Tools.GetTotalMilliseconds(x.DateTime),
                                                Y = (x.balances.unconverted + x.balances.confirmed)
                                            }).ToArray())
                        }
                    });


              
                model.Chart4 = new DotNet.Highcharts.Highcharts("chart4")
                  .InitChart(new Chart { DefaultSeriesType = ChartTypes.Spline })
                  .SetOptions(new GlobalOptions { Global = new Global { UseUTC = false } })
                  .SetTitle(new Title { Text = "Bitcoins per MH/s" })
                  .SetXAxis(new XAxis
                  {
                      Type = AxisTypes.Datetime,

                  })
                  .SetYAxis(new YAxis
                  {
                      Title = new YAxisTitle { Text = "Bitcoins" },

                  })

                  .SetSeries(new[]
                    {
                        new Series
                        {
                            Name = "Bitcoins per MH/s",
                            Data =
                                new Data(
                                    bcPerMh.ToList().Select(
                                        x =>
                                            new Point()
                                            {
                                                X = Tools.GetTotalMilliseconds(x.DateTime),
                                                Y = x.Amount
                                            }).ToArray())
                        }
                    });

                model.Chart3 = new DotNet.Highcharts.Highcharts("chart3")
                  .InitChart(new Chart { DefaultSeriesType = ChartTypes.Spline })
                  .SetOptions(new GlobalOptions { Global = new Global { UseUTC = false } })
                  .SetTitle(new Title { Text = "Recent payouts" })
                  .SetXAxis(new XAxis
                  {
                      Type = AxisTypes.Datetime,

                  })
                  .SetYAxis(new YAxis
                  {
                      Title = new YAxisTitle { Text = "Bitcoins" },

                  })

                  .SetSeries(new[]
                    {
                        new Series
                        {
                            Name = "Recent payouts",
                            Data =
                                new Data(
                                    recentPayouts.ToList().Select(
                                        x =>
                                            new Point()
                                            {
                                                X = Tools.GetTotalMilliseconds(x.DateTime),
                                                Y = x.Amount
                                            }).ToArray())
                        }
                    });
            }
            return View(model);
        }
    }
}
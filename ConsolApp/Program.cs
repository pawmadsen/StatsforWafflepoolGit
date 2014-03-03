using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StatsForWafflepool.Models;


namespace ConsolApp
{
    class Program
    {
        static void Main(string[] args)
        {
             Console.WriteLine("started");
            var sw = new Stopwatch();
            while (true)
            {
               sw.Start();
                using (var db = new ApplicationDbContext())
                {
                    using (var client = new System.Net.WebClient())
                    {
                         foreach (var user in db.Users)
                    {
                        if(user.Address.Length <10)
                            break;

                        var bytes = client.DownloadData(new Uri("http://wafflepool.com/tmp_api?address="+user.Address));
                        var str = Encoding.Default.GetString(bytes);
                        var item = JsonConvert.DeserializeObject<RootObject>(str);
                        if (item.error == "")
                        {
                            item.DateTime = DateTime.UtcNow;
                            item.User = user;
                            db.RootObjects.Add(item);
                        }
                        
                        

                    }
                    }
                   
                    db.SaveChanges();
                 
                }
                sw.Stop();
                if (sw.ElapsedMilliseconds < (1000*60*1))
                {
                    var elapsed = sw.Elapsed;
                    Console.WriteLine(elapsed);
                    sw.Reset();
                    var sleep = (1000*60*5) - Convert.ToInt32(elapsed.TotalSeconds);
                    Thread.Sleep(sleep);
                }
                
                
            }
           
        }
    }
}

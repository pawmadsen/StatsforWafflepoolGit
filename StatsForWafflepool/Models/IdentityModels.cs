using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace StatsForWafflepool.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Address { get; set; }
        public virtual ICollection<RootObject> RootObjects { get; set; }
        
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("Server=sky-server;Database=StatsForWafflepoolDb;User Id=pawmadsen; Password=Sexy1234")
        {
        }

        public DbSet<RecentPayment> RecentPayments { get; set; }
        public DbSet<Balances> Balances { get; set; }
        public DbSet<RootObject> RootObjects { get; set; }
    }
}
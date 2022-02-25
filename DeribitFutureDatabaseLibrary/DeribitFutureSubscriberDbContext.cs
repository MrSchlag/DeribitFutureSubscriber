using DeribitFutureDatabaseLibrary;
using DeribitFutureSubscriber.DbModels;
using Microsoft.EntityFrameworkCore;

namespace DeribitFutureSubscriber
{
    public class DeribitFutureSubscriberDbContext : DbContext
    {
        public DeribitFutureSubscriberDbContext() : base()
        {
            FutureTickers = Set<FutureTicker>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder option)
        {
            option.UseNpgsql(Connexion.ConnexionString);
        }

        public DbSet<FutureTicker> FutureTickers { get; set; }
    }
}

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
            option.UseNpgsql("User ID=postgres;Password=mysecretpassword;Host=postgres;Port=5432;Database=MarketData2;Pooling=true;");
        }

        public DbSet<FutureTicker> FutureTickers { get; set; }
    }
}

using DeribitFutureSubscriber.DbModels;
using Microsoft.EntityFrameworkCore;

namespace DeribitFutureSubscriber
{
    public class DeribitFutureSubscriberDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder option)
        {
            option.UseNpgsql("User ID=postgres;Password=mysecretpassword;Host=localhost;Port=5432;Database=MarketData;Pooling=true;");
        }

        public DbSet<InsturmentTicker> InstrumentTikers { get; set; }
    }
}

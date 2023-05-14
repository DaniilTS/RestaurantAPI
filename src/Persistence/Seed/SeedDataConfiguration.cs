using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Seed.Data;

namespace Persistence.Seed
{
    public static class SeedDataConfiguration
    {
        public static void ConfigureSeed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Table>().HasData(TableSeed.Data);
        }
    }
}

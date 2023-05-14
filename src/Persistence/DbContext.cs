using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Seed;

namespace Persistence
{
    public class DbContext: Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<Table> Tables { get; set; }

        public DbSet<ClientsGroup> ClientsGroups { get; set; }

        public DbContext(DbContextOptions<DbContext> options) : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureSeed();
        }
    }
}

using LocaCraftAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LocaCraftAPI.LocaCraftAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<RealEstateAsset> RealEstateAssets { get; set; }

        public DbSet<Lease> Leases { get; set; }

        public DbSet<Tenant> Tenants { get; set; }

        public DbSet<Lessor> Lessors { get; set; }

        public DbSet<LeaseDocuments> LeaseDocuments { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}

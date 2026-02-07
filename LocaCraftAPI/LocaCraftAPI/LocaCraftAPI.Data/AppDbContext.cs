using LocaCraftAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LocaCraftAPI.LocaCraftAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<RealEstateAsset> RealEstateAssets { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
    }
}

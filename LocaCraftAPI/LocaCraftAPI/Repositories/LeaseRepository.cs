using LocaCraftAPI.LocaCraftAPI.Data;
using LocaCraftAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LocaCraftAPI.Repositories
{
    /// <summary>
    /// EF Core implementation of <see cref="ILeaseRepository"/>.
    /// </summary>
    public class LeaseRepository : ILeaseRepository
    {
        #region Attributes
        // EF Core DbContext for persistence.
        private readonly AppDbContext _context;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new repository instance.
        /// </summary>
        public LeaseRepository(AppDbContext context)
        {
            _context = context;
        }
        #endregion

        /// <inheritdoc />
        public async Task CreateAsync(Lease lease)
        {
            await _context.Leases.AddAsync(lease);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task DeleteAsync(int id)
        {
            var lease = await _context.Leases.FindAsync(id);
            if (lease == null) return;
            _context.Leases.Remove(lease);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Lease>> GetAllAsync()
        {
            return await _context.Leases
                .Include(l => l.RealEstateAsset)
                .Include(l => l.Lessor)
                .Include(l => l.Tenants)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Lease?> GetByIdAsync(int id)
        {
            return await _context.Leases
                .Include(l => l.RealEstateAsset)
                .Include(l => l.Lessor)
                .Include(l => l.Tenants)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Lease>> GetByRealEstateAssetIdAsync(int realEstateAssetId)
        {
            return await _context.Leases
                .Include(l => l.RealEstateAsset)
                .Include(l => l.Lessor)
                .Include(l => l.Tenants)
                .Where(l => l.RealEstateAssetId == realEstateAssetId).ToListAsync();
        }

        /// <inheritdoc />
        public Task UpdateAsync(Lease lease)
        {
            var tracked = _context.ChangeTracker.Entries<Lease>()
                .FirstOrDefault(e => e.Entity.Id == lease.Id);
            if (tracked != null)
                tracked.State = EntityState.Detached;
            _context.Leases.Update(lease);
            return _context.SaveChangesAsync();
        }
    }
}

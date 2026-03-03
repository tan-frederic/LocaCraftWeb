using LocaCraftAPI.LocaCraftAPI.Data;
using LocaCraftAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LocaCraftAPI.Repositories
{
    /// <summary>
    /// EF Core implementation of <see cref="IRealEstateAssetRepository"/>.
    /// </summary>
    public class RealEstateAssetRepository : IRealEstateAssetRepository
    {
        #region Attributes
        // EF Core DbContext for persistence.
        private readonly AppDbContext _context;
        #endregion

        /// <summary>
        /// Initializes a new repository instance.
        /// </summary>
        public RealEstateAssetRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task CreateAsync(RealEstateAsset asset)
        {
            await _context.RealEstateAssets.AddAsync(asset);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<RealEstateAsset>> GetAllAsync()
        {
            return await _context.RealEstateAssets.ToListAsync();
        }

        /// <inheritdoc />
        public async Task<RealEstateAsset?> GetByIdAsync(int id)
        {
            return await _context.RealEstateAssets.FindAsync(id);
        }

        /// <inheritdoc />
        public async Task DeleteAsync(int id)
        {
            var realEstateAsset = await _context.RealEstateAssets.FindAsync(id);
            if (realEstateAsset == null) return;
            _context.RealEstateAssets.Remove(realEstateAsset);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task UpdateAsync(RealEstateAsset asset)
        {
            var tracked = _context.ChangeTracker.Entries<RealEstateAsset>()
                .FirstOrDefault(e => e.Entity.Id == asset.Id);
            if (tracked != null)
                tracked.State = EntityState.Detached;
            _context.RealEstateAssets.Update(asset);
            await _context.SaveChangesAsync();
        }
    }
}

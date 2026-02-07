using LocaCraftAPI.LocaCraftAPI.Data;
using LocaCraftAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LocaCraftAPI.Repositories
{
    public class RealEstateAssetRepository : IRealEstateAssetRepository
    {
        private readonly AppDbContext _context;

        public RealEstateAssetRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(RealEstateAsset asset)
        {
            await _context.RealEstateAssets.AddAsync(asset);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<RealEstateAsset>> GetAllAsync()
        {
            return await _context.RealEstateAssets.ToListAsync();
        }

        public async Task<RealEstateAsset?> GetByIdAsync(int id)
        {
            return await _context.RealEstateAssets.FindAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            var realEstateAsset = await _context.RealEstateAssets.FindAsync(id);
            if (realEstateAsset == null)
                throw new KeyNotFoundException($"RealEstateAsset with id {id} not found.");
            _context.RealEstateAssets.Remove(realEstateAsset);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RealEstateAsset asset)
        {
            _context.RealEstateAssets.Update(asset);
            await _context.SaveChangesAsync();
        }
    }
}

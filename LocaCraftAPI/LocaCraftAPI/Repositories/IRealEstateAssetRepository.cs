using LocaCraftAPI.Models;

namespace LocaCraftAPI.Repositories
{
    public interface IRealEstateAssetRepository
    {
        public Task<IEnumerable<RealEstateAsset>> GetAllAsync();
        public Task<RealEstateAsset?> GetByIdAsync(int id);
        public Task CreateAsync(RealEstateAsset asset);
        public Task UpdateAsync(RealEstateAsset asset);
        public Task DeleteAsync(int id);
    }
}

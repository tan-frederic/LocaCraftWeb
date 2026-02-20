using LocaCraftAPI.Models;

namespace LocaCraftAPI.Repositories
{
    /// <summary>
    /// Repository contract for real estate asset persistence.
    /// </summary>
    public interface IRealEstateAssetRepository
    {
        /// <summary>
        /// Returns all assets.
        /// </summary>
        public Task<IEnumerable<RealEstateAsset>> GetAllAsync();
        /// <summary>
        /// Returns an asset by id or null if not found.
        /// </summary>
        public Task<RealEstateAsset?> GetByIdAsync(int id);
        /// <summary>
        /// Creates a new asset.
        /// </summary>
        public Task CreateAsync(RealEstateAsset asset);
        /// <summary>
        /// Updates an existing asset.
        /// </summary>
        public Task UpdateAsync(RealEstateAsset asset);
        /// <summary>
        /// Deletes an asset by id.
        /// </summary>
        public Task DeleteAsync(int id);
    }
}

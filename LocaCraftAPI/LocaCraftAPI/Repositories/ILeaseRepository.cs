using LocaCraftAPI.Models;

namespace LocaCraftAPI.Repositories
{
    /// <summary>
    /// Repository contract for lease persistence.
    /// </summary>
    public interface ILeaseRepository
    {
        /// <summary>
        /// Returns all leases.
        /// </summary>
        public Task<IEnumerable<Lease>> GetAllAsync();

        /// <summary>
        /// Returns a lease by id or null if not found.
        /// </summary>
        public Task<Lease?> GetByIdAsync(int id);

        public Task<IEnumerable<Lease>> GetByRealEstateAssetIdAsync(int realEstateAssetId);

        /// <summary>
        /// Creates a new lease.
        /// </summary>
        public Task CreateAsync(Lease lease);

        /// <summary>
        /// Updates an existing lease.
        /// </summary>
        public Task UpdateAsync(Lease lease);

        /// <summary>
        /// Deletes a lease by id.
        /// </summary>
        public Task DeleteAsync(int id);
    }
}

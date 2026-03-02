using LocaCraftAPI.Models;

namespace LocaCraftAPI.Repositories
{
    /// <summary>
    /// Repository contract for tenant persistence.
    /// </summary>
    public interface ITenantRepository
    {
        /// <summary>
        /// Returns all tenants.
        /// </summary>
        public Task<IEnumerable<Tenant>> GetAllAsync();

        /// <summary>
        /// Returns a tenant by id or null if not found.
        /// </summary>
        public Task<Tenant?> GetByIdAsync(int id);

        /// <summary>
        /// Returns the tenant linked to a lease id or null if not found.
        /// </summary>
        public Task<Tenant?> GetByLeaseIdAsync(int leaseId);

        /// <summary>
        /// Returns all tenants linked to a lease id.
        /// </summary>
        public Task<IEnumerable<Tenant>> GetAllByLeaseIdAsync(int leaseId);

        /// <summary>
        /// Creates a new tenant.
        /// </summary>
        public Task CreateAsync(Tenant tenant);

        /// <summary>
        /// Updates an existing tenant.
        /// </summary>
        public Task UpdateAsync(Tenant tenant);

        /// <summary>
        /// Deletes a tenant by id.
        /// </summary>
        public Task DeleteAsync(int id);
    }
}

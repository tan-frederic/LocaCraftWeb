using LocaCraftAPI.Models;

namespace LocaCraftAPI.Repositories
{
    public interface ITenantRepository
    {
        public Task<IEnumerable<Tenant>> GetAllAsync();

        public Task<Tenant?> GetByIdAsync(int id);

        public Task CreateAsync(Tenant tenant);

        public Task UpdateAsync(Tenant tenant);

        public Task DeleteAsync(int id);
    }
}

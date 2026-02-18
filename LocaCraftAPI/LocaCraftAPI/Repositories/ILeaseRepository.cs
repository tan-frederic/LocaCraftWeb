using LocaCraftAPI.Models;

namespace LocaCraftAPI.Repositories
{
    public interface ILeaseRepository
    {
        public Task<IEnumerable<Lease>> GetAllAsync();

        public Task<Lease?> GetByIdAsync(int id);

        public Task CreateAsync(Lease lease);

        public Task UpdateAsync(Lease lease);

        public Task DeleteAsync(int id);
    }
}

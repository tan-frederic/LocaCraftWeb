using LocaCraftAPI.Models;

namespace LocaCraftAPI.Repositories
{
    public interface ILessorRepository
    {
        public Task<IEnumerable<Lessor>> GetAllAsync();
        public Task<Lessor?> GetByIdAsync(int id);
        public Task<Lessor?> GetByLeaseId(int leaseId);
        public Task CreateAsync(Lessor lessor);
        public Task UpdateAsync(Lessor lessor);
        public Task DeleteAsync(int id);
    }
}

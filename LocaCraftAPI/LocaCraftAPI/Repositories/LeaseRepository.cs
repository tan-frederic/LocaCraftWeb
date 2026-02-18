using LocaCraftAPI.LocaCraftAPI.Data;
using LocaCraftAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LocaCraftAPI.Repositories
{
    public class LeaseRepository : ILeaseRepository
    {
        #region Attributes
        private readonly AppDbContext _context;
        #endregion

        #region Constructor
        public LeaseRepository(AppDbContext context)
        {
            _context = context;
        }
        #endregion

        public async Task CreateAsync(Lease lease)
        {
            await _context.Leases.AddAsync(lease);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var lease = await _context.Leases.FindAsync(id);
            if (lease == null)
                throw new KeyNotFoundException($"Lease with id {id} not found.");
            _context.Leases.Remove(lease);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Lease>> GetAllAsync()
        {
            return await _context.Leases.ToListAsync();
        }

        public async Task<Lease?> GetByIdAsync(int id)
        {
            return await _context.Leases.FindAsync(id);
        }

        public Task UpdateAsync(Lease lease)
        {
            _context.Leases.Update(lease);
            return _context.SaveChangesAsync();
        }
    }
}

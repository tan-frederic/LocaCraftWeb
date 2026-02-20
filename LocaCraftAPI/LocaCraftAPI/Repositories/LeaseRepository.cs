using LocaCraftAPI.LocaCraftAPI.Data;
using LocaCraftAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LocaCraftAPI.Repositories
{
    /// <summary>
    /// EF Core implementation of <see cref="ILeaseRepository"/>.
    /// </summary>
    public class LeaseRepository : ILeaseRepository
    {
        #region Attributes
        // EF Core DbContext for persistence.
        private readonly AppDbContext _context;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new repository instance.
        /// </summary>
        public LeaseRepository(AppDbContext context)
        {
            _context = context;
        }
        #endregion

        /// <inheritdoc />
        public async Task CreateAsync(Lease lease)
        {
            await _context.Leases.AddAsync(lease);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task DeleteAsync(int id)
        {
            var lease = await _context.Leases.FindAsync(id);
            if (lease == null)
                throw new KeyNotFoundException($"Lease with id {id} not found.");
            _context.Leases.Remove(lease);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Lease>> GetAllAsync()
        {
            return await _context.Leases.ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Lease?> GetByIdAsync(int id)
        {
            return await _context.Leases.FindAsync(id);
        }

        /// <inheritdoc />
        public Task UpdateAsync(Lease lease)
        {
            _context.Leases.Update(lease);
            return _context.SaveChangesAsync();
        }
    }
}

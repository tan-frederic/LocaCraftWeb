using LocaCraftAPI.LocaCraftAPI.Data;
using LocaCraftAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LocaCraftAPI.Repositories
{
    public class LessorRepository : ILessorRepository
    {
        #region Attributes
        private readonly AppDbContext _context;
        #endregion

        #region Constructor
        public LessorRepository(AppDbContext context)
        {
            _context = context;
        }
        #endregion

        public async Task CreateAsync(Lessor lessor)
        {
            await _context.Lessors.AddAsync(lessor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var lessor = await _context.Lessors.FindAsync(id);
            if (lessor == null)
                throw new KeyNotFoundException($"Lessor with ID {id} not found.");
            _context.Lessors.Remove(lessor);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Lessor>> GetAllAsync()
        {
            return await _context.Lessors.ToListAsync();
        }

        public async Task<Lessor?> GetByIdAsync(int id)
        {
            return await _context.Lessors.FindAsync(id);
        }

        public Task<Lessor?> GetByLeaseId(int leaseId)
        {
            return _context.Lessors
                .Include(l => l.Leases)
                .Where(l => l.Leases.Any(lease => lease.Id == leaseId))
                .FirstOrDefaultAsync();
        }

        public Task UpdateAsync(Lessor lessor)
        {
            _context.Lessors.Update(lessor);
            return _context.SaveChangesAsync();
        }
    }
}

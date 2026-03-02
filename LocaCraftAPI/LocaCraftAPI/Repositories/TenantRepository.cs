using LocaCraftAPI.LocaCraftAPI.Data;
using LocaCraftAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LocaCraftAPI.Repositories
{
    /// <summary>
    /// EF Core implementation of <see cref="ITenantRepository"/>.
    /// </summary>
    public class TenantRepository : ITenantRepository
    {
        #region Attributes
        // EF Core DbContext for persistence.
        private readonly AppDbContext _context;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new repository instance.
        /// </summary>
        public TenantRepository(AppDbContext context)
        {
            _context = context;
        }
        #endregion

        /// <inheritdoc />
        public async Task CreateAsync(Tenant tenant)
        {
            await _context.Tenants.AddAsync(tenant);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task DeleteAsync(int id)
        {
            var tenant = await _context.Tenants.FindAsync(id);
            if (tenant == null) return;
            _context.Tenants.Remove(tenant);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Tenant>> GetAllAsync()
        {
            return await _context.Tenants.ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Tenant?> GetByIdAsync(int id)
        {
            return await _context.Tenants.FindAsync(id);
        }

        /// <inheritdoc />
        public async Task<Tenant?> GetByLeaseIdAsync(int leaseId)
        {
            return await _context.Tenants.FirstOrDefaultAsync(t => t.LeaseId == leaseId);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Tenant>> GetAllByLeaseIdAsync(int leaseId)
        {
            return await _context.Tenants.Where(t => t.LeaseId == leaseId).ToListAsync();
        }

        /// <inheritdoc />
        public Task UpdateAsync(Tenant tenant)
        {
            _context.Tenants.Update(tenant);
            return _context.SaveChangesAsync();
        }
    }
}

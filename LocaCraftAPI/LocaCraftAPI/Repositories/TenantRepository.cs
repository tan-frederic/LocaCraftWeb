using LocaCraftAPI.LocaCraftAPI.Data;
using LocaCraftAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LocaCraftAPI.Repositories
{
    public class TenantRepository : ITenantRepository
    {
        #region Attributes
        private readonly AppDbContext _context;
        #endregion

        #region Constructor
        public TenantRepository(AppDbContext context)
        {
            _context = context;
        }
        #endregion

        public async Task CreateAsync(Tenant tenant)
        {
            await _context.Tenants.AddAsync(tenant);
            await _context.SaveChangesAsync();
        }

        public Task DeleteAsync(int id)
        {
            var tenant = _context.Tenants.Find(id);
            if (tenant == null)
                throw new KeyNotFoundException($"Tenant with id {id} not found.");
            _context.Tenants.Remove(tenant);
            return _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Tenant>> GetAllAsync()
        {
            return await _context.Tenants.ToListAsync();
        }

        public async Task<Tenant?> GetByIdAsync(int id)
        {
            return await _context.Tenants.FindAsync(id);
        }

        public Task UpdateAsync(Tenant tenant)
        {
            _context.Tenants.Update(tenant);
            return _context.SaveChangesAsync();
        }
    }
}

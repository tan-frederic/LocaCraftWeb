using LocaCraftAPI.Models;
using LocaCraftAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LocaCraftAPI.Controllers
{
    /// <summary>
    /// CRUD endpoints for tenant resources.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        #region Attributes
        // Repository abstraction for tenant persistence.
        public readonly ITenantRepository _tenantRepository;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new controller instance.
        /// </summary>
        public TenantController(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }
        #endregion

        #region CRUD Operations

        /// <summary>
        /// Returns all tenants.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tenant>>> GetAllTenants()
        {
            var tenants = await _tenantRepository.GetAllAsync();
            return Ok(tenants);
        }

        /// <summary>
        /// Returns a single tenant by id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Tenant>> GetTenantById(int id)
        {
            var tenant = await _tenantRepository.GetByIdAsync(id);
            if (tenant == null)
                return NotFound();
            return Ok(tenant);
        }

        [HttpGet("lease/{leaseId}")]
        public async Task<ActionResult<Tenant>> GetTenantByLeaseId(int leaseId)
        {
            var tenant = await _tenantRepository.GetByLeaseIdAsync(leaseId);
            if (tenant == null)
                return NotFound();
            return Ok(tenant);
        }

        [HttpGet("lease/{leaseId}/all")]
        public async Task<ActionResult<IEnumerable<Tenant>>> GetTenantsByLeaseId(int leaseId)
        {
            var tenants = await _tenantRepository.GetAllByLeaseIdAsync(leaseId);
            return Ok(tenants);
        }

        /// <summary>
        /// Creates a new tenant.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Tenant>> CreateTenant(Models.Tenant tenant)
        {
            await _tenantRepository.CreateAsync(tenant);
            return CreatedAtAction(nameof(GetTenantById), new { id = tenant.Id }, tenant);
        }

        /// <summary>
        /// Updates an existing tenant.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<Tenant>> UpdateTenant(int id, Tenant tenant)
        {
            // Ensure route id and payload id match to avoid unintended updates.
            if (id != tenant.Id)
                return BadRequest();
            var existingTenant = await _tenantRepository.GetByIdAsync(id);
            if (existingTenant == null)
                return NotFound();
            await _tenantRepository.UpdateAsync(tenant);
            return CreatedAtAction(nameof(GetTenantById), new { id = tenant.Id }, tenant);
        }

        /// <summary>
        /// Deletes a tenant by id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existingTenant = await _tenantRepository.GetByIdAsync(id);
            if (existingTenant == null)
                return NotFound();
            await _tenantRepository.DeleteAsync(id);
            return NoContent();
        }

        #endregion
    }
}

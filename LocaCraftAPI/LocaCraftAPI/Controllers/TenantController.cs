using LocaCraftAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LocaCraftAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        #region Attributes
        public readonly ITenantRepository _tenantRepository;
        #endregion

        #region Constructor
        public TenantController(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }
        #endregion

        #region CRUD Operations

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Tenant>>> GetAllTenants()
        {
            var tenants = await _tenantRepository.GetAllAsync();
            return Ok(tenants);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Tenant>> GetTenantById(int id)
        {
            var tenant = await _tenantRepository.GetByIdAsync(id);
            if (tenant == null)
                return NotFound();
            return Ok(tenant);
        }

        [HttpPost]
        public async Task<ActionResult<Models.Tenant>> CreateTenant(Models.Tenant tenant)
        {
            await _tenantRepository.CreateAsync(tenant);
            return CreatedAtAction(nameof(GetTenantById), new { id = tenant.Id }, tenant);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Models.Tenant>> UpdateTenant(int id, Models.Tenant tenant)
        {
            if (id != tenant.Id)
                return BadRequest();
            var existingTenant = await _tenantRepository.GetByIdAsync(id);
            if (existingTenant == null)
                return NotFound();
            await _tenantRepository.UpdateAsync(tenant);
            return CreatedAtAction(nameof(GetTenantById), new { id = tenant.Id }, tenant);
        }

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

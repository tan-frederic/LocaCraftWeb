using LocaCraftAPI.DTOs.Tenant;
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
        private readonly ITenantRepository _tenantRepository;
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
        public async Task<ActionResult<IEnumerable<TenantResponseDTO>>> GetAllTenants()
        {
            var tenants = await _tenantRepository.GetAllAsync();
            var dtos = tenants.Select(TenantMapper.ToResponseDTO);
            return Ok(dtos);
        }

        /// <summary>
        /// Returns a single tenant by id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<TenantResponseDTO>> GetTenantById(int id)
        {
            var tenant = await _tenantRepository.GetByIdAsync(id);
            if (tenant == null)
                return NotFound();
            return Ok(TenantMapper.ToResponseDTO(tenant));
        }

        [HttpGet("lease/{leaseId}")]
        public async Task<ActionResult<TenantResponseDTO>> GetTenantByLeaseId(int leaseId)
        {
            var tenant = await _tenantRepository.GetByLeaseIdAsync(leaseId);
            if (tenant == null)
                return NotFound();
            return Ok(TenantMapper.ToResponseDTO(tenant));
        }

        [HttpGet("lease/{leaseId}/all")]
        public async Task<ActionResult<IEnumerable<TenantResponseDTO>>> GetTenantsByLeaseId(int leaseId)
        {
            var tenants = await _tenantRepository.GetAllByLeaseIdAsync(leaseId);
            var dtos = tenants.Select(TenantMapper.ToResponseDTO);
            return Ok(dtos);
        }

        /// <summary>
        /// Creates a new tenant.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<TenantResponseDTO>> CreateTenant(TenantCreateDTO dto)
        {
            var tenant = TenantMapper.ToEntity(dto);
            await _tenantRepository.CreateAsync(tenant);

            var responseDto = TenantMapper.ToResponseDTO(tenant);
            return CreatedAtAction(nameof(GetTenantById), new { id = responseDto.Id }, responseDto);
        }

        /// <summary>
        /// Updates an existing tenant.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<TenantResponseDTO>> UpdateTenant(int id, TenantCreateDTO tenant)
        {
            var existingTenant = await _tenantRepository.GetByIdAsync(id);
            if (existingTenant == null)
                return NotFound();
            TenantMapper.ApplyUpdate(tenant, existingTenant);
            await _tenantRepository.UpdateAsync(existingTenant);
            return Ok(TenantMapper.ToResponseDTO(existingTenant));
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

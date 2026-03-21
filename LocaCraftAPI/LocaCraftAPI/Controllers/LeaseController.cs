using LocaCraftAPI.DTOs.Lease;
using LocaCraftAPI.Models;
using LocaCraftAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LocaCraftAPI.Controllers
{
    /// <summary>
    /// CRUD endpoints for lease resources.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LeaseController : ControllerBase
    {
        #region Attributes
        // Repository abstraction for lease persistence.
        private readonly ILeaseRepository _leaseRepository;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new controller instance.
        /// </summary>
        public LeaseController(ILeaseRepository leaseRepository)
        {
            _leaseRepository = leaseRepository;
        }
        #endregion

        /// <summary>
        /// Returns all leases.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaseResponseDTO>>> GetAllLeases()
        {
            var leases = await _leaseRepository.GetAllAsync();
            var dtos = leases.Select(LeaseMapper.ToResponseDTO);
            return Ok(dtos);
        }

        /// <summary>
        /// Returns a single lease by id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<LeaseResponseDTO>> GetLeaseById(int id)
        {
            var lease = await _leaseRepository.GetByIdAsync(id);
            if (lease == null)
                return NotFound();
            return Ok(LeaseMapper.ToResponseDTO(lease));
        }

        [HttpGet("realestateasset/{realEstateAssetId}")]
        public async Task<ActionResult<IEnumerable<LeaseResponseDTO>>> GetLeasesByRealEstateAssetId(int realEstateAssetId)
        {
            var leases = await _leaseRepository.GetByRealEstateAssetIdAsync(realEstateAssetId);
            var dtos = leases.Select(LeaseMapper.ToResponseDTO);
            return Ok(dtos);
        }

        /// <summary>
        /// Creates a new lease.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<LeaseResponseDTO>> CreateLease(LeaseCreateDTO dto)
        {
            var lease = LeaseMapper.ToEntity(dto);
            await _leaseRepository.CreateAsync(lease);

            var responseDto = LeaseMapper.ToResponseDTO(lease);
            return CreatedAtAction(nameof(GetLeaseById), new { id = responseDto.Id }, responseDto);
        }

        /// <summary>
        /// Updates an existing lease.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<LeaseResponseDTO>> UpdateLease(int id, LeaseCreateDTO dto)
        {
            var existingLease = await _leaseRepository.GetByIdAsync(id);
            if (existingLease == null)
                return NotFound();

            LeaseMapper.ApplyUpdate(dto, existingLease);
            await _leaseRepository.UpdateAsync(existingLease);
            return Ok(LeaseMapper.ToResponseDTO(existingLease));
        }

        /// <summary>
        /// Deletes a lease by id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existingLease = await _leaseRepository.GetByIdAsync(id);
            if (existingLease == null)
                return NotFound();
            await _leaseRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}

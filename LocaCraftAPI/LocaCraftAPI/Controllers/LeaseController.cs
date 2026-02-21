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
        public async Task<ActionResult<IEnumerable<Lease>>> GetAllLeases()
        {
            var leases = await _leaseRepository.GetAllAsync();
            return Ok(leases);
        }

        /// <summary>
        /// Returns a single lease by id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Lease>> GetLeaseById(int id)
        {
            var lease = await _leaseRepository.GetByIdAsync(id);
            if (lease == null)
                return NotFound();
            return Ok(lease);
        }

        [HttpGet("realestateasset/{realEstateAssetId}")]
        public async Task<ActionResult<IEnumerable<Lease>>> GetLeasesByRealEstateAssetId(int realEstateAssetId)
        {
            var leases = await _leaseRepository.GetByRealEstateAssetIdAsync(realEstateAssetId);
            return Ok(leases);
        }

        /// <summary>
        /// Creates a new lease.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Lease>> CreateLease(Lease lease)
        {
            await _leaseRepository.CreateAsync(lease);
            return CreatedAtAction(nameof(GetLeaseById), new { id = lease.Id }, lease);
        }

        /// <summary>
        /// Updates an existing lease.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<Lease>> UpdateLease(int id, Lease lease)
        {
            // Ensure route id and payload id match to avoid unintended updates.
            if (id != lease.Id)
                return BadRequest();
            var existingLease = await _leaseRepository.GetByIdAsync(id);
            if (existingLease == null)
                return NotFound();
            await _leaseRepository.UpdateAsync(lease);
            return CreatedAtAction(nameof(GetLeaseById), new { id = lease.Id }, lease);
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

using LocaCraftAPI.Models;
using LocaCraftAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LocaCraftAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaseController : ControllerBase
    {
        #region Attributes
        private readonly ILeaseRepository _leaseRepository;
        #endregion

        #region Constructor
        public LeaseController(ILeaseRepository leaseRepository)
        {
            _leaseRepository = leaseRepository;
        }
        #endregion

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lease>>> GetAllLeases()
        {
            var leases = await _leaseRepository.GetAllAsync();
            return Ok(leases);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Lease>> GetLeaseById(int id)
        {
            var lease = await _leaseRepository.GetByIdAsync(id);
            if (lease == null)
                return NotFound();
            return Ok(lease);
        }

        [HttpPost]
        public async Task<ActionResult<Lease>> CreateLease(Lease lease)
        {
            await _leaseRepository.CreateAsync(lease);
            return CreatedAtAction(nameof(GetLeaseById), new { id = lease.Id }, lease);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Lease>> UpdateLease(int id, Lease lease)
        {
            if (id != lease.Id)
                return BadRequest();
            var existingLease = await _leaseRepository.GetByIdAsync(id);
            if (existingLease == null)
                return NotFound();
            await _leaseRepository.UpdateAsync(lease);
            return CreatedAtAction(nameof(GetLeaseById), new { id = lease.Id }, lease);
        }

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

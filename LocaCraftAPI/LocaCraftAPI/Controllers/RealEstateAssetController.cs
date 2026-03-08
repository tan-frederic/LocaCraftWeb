using LocaCraftAPI.Models;
using LocaCraftAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LocaCraftAPI.Controllers
{
    /// <summary>
    /// CRUD endpoints for real estate assets.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RealEstateAssetController : ControllerBase
    {
        // Repository abstraction for asset persistence.
        private readonly IRealEstateAssetRepository _realEstateAssetRepository;

        /// <summary>
        /// Initializes a new controller instance.
        /// </summary>
        public RealEstateAssetController(IRealEstateAssetRepository repository)
        {
            _realEstateAssetRepository = repository;
        }

        private string? GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        /// <summary>
        /// Returns all real estate assets belonging to the authenticated user.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RealEstateAsset>>> GetAllRealEstateAsset()
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();
            var assets = await _realEstateAssetRepository.GetAllAsync(userId);
            return Ok(assets);
        }

        /// <summary>
        /// Returns a single asset by id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<RealEstateAsset>> GetRealEstateAssetById(int id)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();
            var asset = await _realEstateAssetRepository.GetByIdAsync(id);
            if (asset == null)
                return NotFound();
            if (asset.UserId != userId)
                return Forbid();
            return Ok(asset);
        }

        /// <summary>
        /// Creates a new asset.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<RealEstateAsset>> CreateRealEstateAsset(RealEstateAsset asset)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();
            asset.UserId = userId;
            await _realEstateAssetRepository.CreateAsync(asset);
            return CreatedAtAction(nameof(GetRealEstateAssetById), new { id = asset.Id }, asset);
        }

        /// <summary>
        /// Updates an existing asset.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<RealEstateAsset>> UpdateRealEstateAsset(int id, RealEstateAsset asset)
        {
            // Ensure route id and payload id match to avoid unintended updates.
            if (id != asset.Id)
                return BadRequest();
            var userId = GetUserId();
            if (userId == null) return Unauthorized();
            var existingAsset = await _realEstateAssetRepository.GetByIdAsync(id);
            if (existingAsset == null)
                return NotFound();
            if (existingAsset.UserId != userId)
                return Forbid();
            asset.UserId = userId;
            await _realEstateAssetRepository.UpdateAsync(asset);
            return Ok(asset);
        }

        /// <summary>
        /// Deletes an asset by id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();
            var existingAsset = await _realEstateAssetRepository.GetByIdAsync(id);
            if (existingAsset == null)
                return NotFound();
            if (existingAsset.UserId != userId)
                return Forbid();
            await _realEstateAssetRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}

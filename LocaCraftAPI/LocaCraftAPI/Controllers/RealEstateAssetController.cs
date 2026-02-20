using LocaCraftAPI.Models;
using LocaCraftAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LocaCraftAPI.Controllers
{
    /// <summary>
    /// CRUD endpoints for real estate assets.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
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

        /// <summary>
        /// Returns all real estate assets.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RealEstateAsset>>> GetAllRealEstateAsset()
        {
            var assets = await _realEstateAssetRepository.GetAllAsync();
            return Ok(assets);
        }

        /// <summary>
        /// Returns a single asset by id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<RealEstateAsset>> GetRealEstateAssetById(int id)
        {
            var asset = await _realEstateAssetRepository.GetByIdAsync(id);
            if (asset == null)
                return NotFound();
            return Ok(asset);
        }

        /// <summary>
        /// Creates a new asset.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<RealEstateAsset>> CreateRealEstateAsset(RealEstateAsset asset)
        {
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
            var existingAsset = await _realEstateAssetRepository.GetByIdAsync(id);
            if (existingAsset == null)
                return NotFound();
            await _realEstateAssetRepository.UpdateAsync(asset);
            return CreatedAtAction(nameof(GetRealEstateAssetById), new { id = asset.Id }, asset);
        }

        /// <summary>
        /// Deletes an asset by id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existingAsset = await _realEstateAssetRepository.GetByIdAsync(id);
            if (existingAsset == null)
                return NotFound();
            await _realEstateAssetRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}

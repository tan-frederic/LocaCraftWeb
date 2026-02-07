using LocaCraftAPI.Models;
using LocaCraftAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LocaCraftAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RealEstateAssetController : ControllerBase
    {
        private readonly IRealEstateAssetRepository _realEstateAssetRepository;

        public RealEstateAssetController(IRealEstateAssetRepository repository)
        {
            _realEstateAssetRepository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RealEstateAsset>>> GetAllRealEstateAsset()
        {
            var assets = await _realEstateAssetRepository.GetAllAsync();
            return Ok(assets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RealEstateAsset>> GetRealEstateAssetById(int id)
        {
            var asset = await _realEstateAssetRepository.GetByIdAsync(id);
            if (asset == null)
                return NotFound();
            return Ok(asset);
        }

        [HttpPost]
        public async Task<ActionResult<RealEstateAsset>> CreateRealEstateAsset(RealEstateAsset asset)
        {
            await _realEstateAssetRepository.CreateAsync(asset);
            return CreatedAtAction(nameof(GetRealEstateAssetById), new { id = asset.Id }, asset);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<RealEstateAsset>> UpdateRealEstateAsset(int id, RealEstateAsset asset)
        {
            if (id != asset.Id)
                return BadRequest();
            var existingAsset = await _realEstateAssetRepository.GetByIdAsync(id);
            if (existingAsset == null)
                return NotFound();
            await _realEstateAssetRepository.UpdateAsync(asset);
            return CreatedAtAction(nameof(GetRealEstateAssetById), new { id = asset.Id }, asset);
        }

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

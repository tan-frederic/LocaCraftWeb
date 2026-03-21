using LocaCraftAPI.DTOs.RealEstateAsset;
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
        public async Task<ActionResult<IEnumerable<RealEstateAssetResponseDTO>>> GetAllRealEstateAsset()
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized();
            var assets = await _realEstateAssetRepository.GetAllAsync(userId);
            var dtos = assets.Select(RealEstateAssetMapper.ToResponseDTO);
            return Ok(dtos);
        }

        /// <summary>
        /// Returns a single asset by id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<RealEstateAssetResponseDTO>> GetRealEstateAssetById(int id)
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized();
            var asset = await _realEstateAssetRepository.GetByIdAsync(id);
            if (asset == null)
                return NotFound();
            if (asset.UserId != userId)
                return Forbid();
            return Ok(RealEstateAssetMapper.ToResponseDTO(asset));
        }

        /// <summary>
        /// Creates a new asset.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<RealEstateAssetResponseDTO>> CreateRealEstateAsset(RealEstateAssetCreateDTO dto)
        {
            var userId = GetUserId();
            if (userId == null) 
               return Unauthorized();
            var asset = RealEstateAssetMapper.ToEntity(dto);
            asset.UserId = userId;
            await _realEstateAssetRepository.CreateAsync(asset);

            var responseDto = RealEstateAssetMapper.ToResponseDTO(asset);
            return CreatedAtAction(nameof(GetRealEstateAssetById), new { id = responseDto.Id }, responseDto);
        }

        /// <summary>
        /// Updates an existing asset.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<RealEstateAssetResponseDTO>> UpdateRealEstateAsset(int id, RealEstateAssetCreateDTO dto)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();
            var existingAsset = await _realEstateAssetRepository.GetByIdAsync(id);
            if (existingAsset == null)
                return NotFound();
            if (existingAsset.UserId != userId)
                return Forbid();
            RealEstateAssetMapper.ApplyUpdate(dto, existingAsset);
            existingAsset.UserId = userId;
            await _realEstateAssetRepository.UpdateAsync(existingAsset);
            return Ok(RealEstateAssetMapper.ToResponseDTO(existingAsset));
        }

        /// <summary>
        /// Deletes an asset by id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var userId = GetUserId();
            if (userId == null)
                return Unauthorized();
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

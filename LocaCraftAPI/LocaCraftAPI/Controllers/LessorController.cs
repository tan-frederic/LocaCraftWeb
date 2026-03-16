using LocaCraftAPI.DTOs.Lessor;
using LocaCraftAPI.Models;
using LocaCraftAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LocaCraftAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessorController : ControllerBase
    {
        #region Attributes
        private readonly ILessorRepository _lessorRepository;
        #endregion

        #region Constructor
        public LessorController(ILessorRepository lessorRepository)
        {
            _lessorRepository = lessorRepository;
        }
        #endregion

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LessorResponseDTO>>> GetAllLessors()
        {
            var lessors = await _lessorRepository.GetAllAsync();
            var dtos = lessors.Select(LessorMapper.ToResponseDTO);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LessorResponseDTO>> GetLessorById(int id)
        {
            var lessor = await _lessorRepository.GetByIdAsync(id);
            if (lessor == null)
                return NotFound();
            return Ok(LessorMapper.ToResponseDTO(lessor));
        }

        [HttpPost]
        public async Task<ActionResult<LessorResponseDTO>> CreateLessor(LessorCreateDTO dto)
        {
            var lessor = LessorMapper.ToEntity(dto);
            await _lessorRepository.CreateAsync(lessor);

            var responseDto = LessorMapper.ToResponseDTO(lessor);
            return CreatedAtAction(nameof(GetLessorById), new { id = responseDto.Id }, responseDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<LessorResponseDTO>> UpdateLessor(int id, LessorCreateDTO dto)
        {
            var existingLessor = await _lessorRepository.GetByIdAsync(id);
            if (existingLessor == null)
                return NotFound();

            LessorMapper.ApplyUpdate(dto, existingLessor);
            await _lessorRepository.UpdateAsync(existingLessor);
            return Ok(LessorMapper.ToResponseDTO(existingLessor));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLessor(int id)
        {
            var existingLessor = await _lessorRepository.GetByIdAsync(id);
            if (existingLessor == null)
                return NotFound();
            await _lessorRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}

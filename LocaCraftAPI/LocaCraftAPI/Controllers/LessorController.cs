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
        public readonly ILessorRepository _lessorRepository;
        #endregion

        #region Constructor
        public LessorController(ILessorRepository lessorRepository)
        {
            _lessorRepository = lessorRepository;
        }
        #endregion

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lessor>>> GetAllLessors()
        {
            var lessors = await _lessorRepository.GetAllAsync();
            return Ok(lessors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Lessor>> GetLessorById(int id)
        {
            var lessor = await _lessorRepository.GetByIdAsync(id);
            if (lessor == null)
                return NotFound();
            return Ok(lessor);
        }

        [HttpPost]
        public async Task<ActionResult<Lessor>> CreateLessor(Lessor lessor)
        {
            await _lessorRepository.CreateAsync(lessor);
            return CreatedAtAction(nameof(GetLessorById), new { id = lessor.Id }, lessor);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Lessor>> UpdateLessor(int id, Lessor lessor)
        {
            if (id != lessor.Id)
                return BadRequest();
            var existingLessor = await _lessorRepository.GetByIdAsync(id);
            if (existingLessor == null)
                return NotFound();
            await _lessorRepository.UpdateAsync(lessor);
            return CreatedAtAction(nameof(GetLessorById), new { id = lessor.Id }, lessor);
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

using LocaCraftAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocaCraftAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InseeIndexController : ControllerBase
    {
        #region Attributes
        private readonly IInseeService _inseeService;
        #endregion

        #region Constructor
        public InseeIndexController(IInseeService inseeService)
        {
            _inseeService = inseeService;
        }
        #endregion

        [HttpGet]
        public async Task<ActionResult> GetIndex([FromQuery] int LastNIndex = 20)
        {
            try 
            {
                var data = await _inseeService.GetIndexAsync(LastNIndex);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error fetching INSEE index data: {ex.Message}");
            }
        }
    }
}

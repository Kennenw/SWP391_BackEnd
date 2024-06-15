using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.DTO;
using Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingBad.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourtsController : ControllerBase
    {
        private readonly ICourtServices _courtServices;

        public CourtsController()
        {
            _courtServices = new CourtServices();
        }

        // GET: api/Courts
        [HttpGet]
        public ActionResult<IEnumerable<CourtDTOs>> GetCourt(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = _courtServices.GetCourts( pageNumber, pageSize);
            if (result == null)
            {
                return BadRequest(new { message = "No Court to find" });
            }
            return Ok(result);
        }

        // GET: api/Courts/Search
        [HttpGet("Search-Court")]
        public async Task<ActionResult<PagedResult<CourtDTO>>> SearchCourts(
            [FromQuery] string searchTerm,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = _courtServices.SearchCourts(searchTerm, pageNumber, pageSize);
            if (result == null || !result.Items.Any())
            {
                return BadRequest(new { message = "No Court to find" });
            }
            return Ok(result);
        }

        // GET: api/Courts/{id}
        [HttpGet("{id}")]
        public ActionResult<CourtDTO> GetCourtById(int id)
        {
            var court = _courtServices.GetCourtById(id);
            if (court == null)
            {
                return NotFound(new { message = "Court not found" });
            }
            return Ok(court);
        }

        // POST: api/Courts/CreateCourt
        [HttpPost]
        public async Task<IActionResult> CreateCourt([FromBody] CourtDTO CreateCourt)
        {
            if (CreateCourt == null)
            {
                return BadRequest("The courtDto field is required.");
            }

            var createdCourt = await _courtServices.CreateCourtAsync(CreateCourt);
            return Ok(new SuccessObject<object> {Data = createdCourt, Message = "Tạo thành công" });
        }

        // PUT: api/Courts/Update/{id}
        [HttpPut("Update/{id}")]
        public ActionResult UpdateCourt(int id, [FromBody] CourtDTOs courtDTO)
        {
            _courtServices.UpdateCourt(id, courtDTO);
            return Ok(new { message = "Court updated successfully" });
        }

        // POST: api/Courts/UploadCourtImage/{courtId}
        [HttpPost("UploadCourtImage/{courtId}")]
        public async Task<IActionResult> UploadCourtImage(int courtId, [FromBody] Base64ImageModel model)
        {
            if (string.IsNullOrEmpty(model.Base64Image))
                return BadRequest("No image uploaded.");

            await _courtServices.UploadCourtImageAsync(courtId, model.Base64Image);

            return Ok("Image uploaded successfully.");
        }

        // DELETE: api/Courts/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteCourt(int id)
        {
            _courtServices.DeleteCourt(id);
            return Ok(new { message = "Court deleted successfully" });
        }
    }
}
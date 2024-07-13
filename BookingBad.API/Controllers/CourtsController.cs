using Microsoft.AspNetCore.Mvc;
using Repositories;
using Repositories.DTO;
using Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookingBad.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourtsController : ControllerBase
    {
        private readonly ICourtServices _courtServices;

        public CourtsController(ICourtServices courtServices)
        {
            _courtServices = courtServices;
        }

        [HttpGet("LatestCreated")]
        public IActionResult GetLatestCreatedCourts(int count)
        {
            try
            {
                var courts = _courtServices.GetLatestCreatedCourts(count);
                if (courts == null || !courts.Any())
                {
                    return NotFound(new { message = "No courts found" });
                }
                return Ok(courts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }

        // GET: api/Courts
        [HttpGet]
        public ActionResult<IEnumerable<CourtDTOs>> GetCourt()
        {
            try { 
            var result = _courtServices.GetCourts();
            if (result == null)
            {
                return BadRequest(new { message = "No Court to find" });
            }
            return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }

        // GET: api/Courts/Search
        [HttpGet("Search-Court")]
        public async Task<ActionResult<IEnumerable<CourtDTO>>> SearchCourts(string? searchTerm, int? areaId)
        {
            try
            {
                var result = _courtServices.SearchCourts(searchTerm, areaId);
                if (result == null || !result.Any())
                {
                    return BadRequest(new { message = "No Court to find" });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }

        // GET: api/Courts/{id}
        [HttpGet("{id}")]
        public ActionResult<CourtGET> GetCourtById(int id)
        {
            try
            {
                var court = _courtServices.GetCourtById(id);
                if (court == null)
                {
                    return NotFound(new { message = "Court not found" });
                }
                return Ok(court);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpGet("GetCourts/{courtId}")]
        public IActionResult GetCourtByIds(int courtId)
        {
            try
            {
                var court = _courtServices.GetCourtById(courtId);
                if (court == null)
                {
                    return NotFound();
                }
                return Ok(court);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }

        // PUT: api/Courts/Update/{id}
        [HttpPut("Update/{id}")]
        public ActionResult UpdateCourt(int id, CourtDTOs courtDTO)
        {
            _courtServices.UpdateCourt(id, courtDTO);
            return Ok(new { message = "Court updated successfully" });
        }

        // POST: api/Courts/UploadCourtImage/{courtId}
        [HttpPut("UploadCourtImage/{courtId}")]
        public async Task<IActionResult> UploadCourtImage(int courtId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No image uploaded.");

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                await _courtServices.UploadCourtImageAsync(courtId, memoryStream.ToArray());
            }

            return Ok("Image uploaded successfully.");
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourtWithImage(CourtDTO courtCreationDTO)
        {
            await _courtServices.CreateCourtAsync(courtCreationDTO);
            return CreatedAtAction("GetCourtByIds", new { courtId = courtCreationDTO.CourtId }, courtCreationDTO);
        }

        // DELETE: api/Courts/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteCourt(int id)
        {
            _courtServices.DeleteCourt(id);
            return Ok(new { message = "Court deleted successfully" });
        }

        [HttpPost("RateCourt/{courtId}")]
        public IActionResult RateCourt(int courtId, [FromBody] RatingCourtDTO model)
        {
            _courtServices.RateCourt(courtId, model.UserId, model.RatingValue);
            return CreatedAtAction("GetCourtByIds", new { courtId = courtId }, courtId);
        }

        [HttpGet("{courtId}/Image")]
        public IActionResult GetCourtImage(int courtId)
        {
            try
            {
                var imagePath = _courtServices.GetCourtImagePath(courtId);
                if (string.IsNullOrEmpty(imagePath))
                {
                    return NotFound(new { message = "Image not found" });
                }
                var image = System.IO.File.ReadAllBytes(imagePath);
                return File(image, "image/png");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Courts/{id}/SlotTimes
        [HttpGet("{courtId}/SlotTimes")]
        public async Task<ActionResult<IEnumerable<SlotTimeDTO>>> GetSlotTimesByDate(int courtId, [FromQuery] DateTime date, [FromQuery] int subCourtId)
        {
            var slotTimes = await _courtServices.GetSlotTimesByDate(courtId, date, subCourtId);
            if (slotTimes == null || !slotTimes.Any())
            {
                return NotFound(new { message = "No slot times found for the selected date and subcourt" });
            }
            return Ok(slotTimes);
        }
    }
}
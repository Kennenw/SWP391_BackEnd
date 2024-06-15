using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.DTO;
using Repositories.Entities;
using Services;

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

        [HttpGet]       
        public ActionResult<IEnumerable<CourtDTO>> GetCourt(
            [FromQuery]int managerId,
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10)
        {
 
            var result = _courtServices.GetCourts(managerId, pageNumber, pageSize);
            if(result == null)
            {
                return BadRequest(new { message = "No Court to find" });
            }return Ok(result);
        }

        // GET: api/Court/Search
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

        // POST: api/Courts/Create
        [HttpPost]
        public async Task<ActionResult<CourtDTO>> CreateCourt(CourtDTO courtCreateDTO)
        {
            try
            {
                await _courtServices.CreateCourtAsync(courtCreateDTO);
                return CreatedAtAction("GetCourtById", new { id = courtCreateDTO.CourtId }, courtCreateDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            
        }

        // PUT: api/Courts/Update/{id}
        [HttpPut("Update/{id}")]
        public ActionResult UpdateCourt(int id, [FromBody] CourtDTO courtDTO)
        {
            _courtServices.UpdateCourt(id, courtDTO);
            return Ok(new { message = "Court updated successfully" });
        }

        [HttpPost("UploadCourtImage/{courtId}")]
        public async Task<IActionResult> UploadCourtImage(int courtId, [FromBody] Base64ImageModel model)
        {
            if (string.IsNullOrEmpty(model.Base64Image))
                return BadRequest("No image uploaded.");

            await _courtServices.UploadCourtImageAsync(courtId, model.Base64Image);

            return Ok("Image uploaded successfully.");
        }

        // DELETE: api/Courts/5
        [HttpDelete("{id}")]
        public ActionResult DeleteCourt(int id)
        {
            _courtServices.DeleteCourt(id);
            return Ok(new { message = "Court deleted successfully" });
        }
    }
}

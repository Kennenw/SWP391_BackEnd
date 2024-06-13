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

        public CourtsController(CourtServices courtServices)
        {
            _courtServices = courtServices;
        }

        [HttpGet]       
        public ActionResult<IEnumerable<CourtDTO>> GetCourt(
            [FromQuery]int managerId,
            [FromQuery]SortCourtByEnum sortCourtBy ,
            [FromQuery]SortTypeEnum sortCourtType ,
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10)
        {
            var sortContent = new SortContent
            {
                sortCourtBy = sortCourtBy,
                sortType = sortCourtType,
            };
            var result = _courtServices.GetCourts(managerId, sortContent, pageNumber, pageSize);
            if(result == null)
            {
                return NotFound();
            }return Ok(result);
        }

        // GET: api/Court/Search
        [HttpGet("Search-Court")]
        public async Task<ActionResult<PagedResult<CourtDTO>>> SearchCourts(
            [FromQuery] string searchTerm,
            [FromQuery] SortCourtByEnum sortCourtBy,
            [FromQuery] SortTypeEnum sortCourtType,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var sortContent = new SortContent
            {
                sortCourtBy = sortCourtBy,
                sortType = sortCourtType
            };

            var result = _courtServices.SearchCourts(searchTerm, sortContent, pageNumber, pageSize);
            if (result == null || !result.Items.Any())
            {
                return NotFound();
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
        [HttpPost("Create")]
        public async Task<IActionResult> CreateCourt(CourtDTO courtCreateDTO)
        {
            try
            {
                await _courtServices.CreateCourtAsync(courtCreateDTO);
                return Ok(new { message = "Court created successfully" });
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

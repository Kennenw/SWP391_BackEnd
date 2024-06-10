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
                return NoContent();
            }return Ok(result);
        }

        // GET: api/Court/Search
        [HttpGet("Search")]
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
        public ActionResult CreateCourt([FromForm] CourtCreateDTO courtCreateDTO)
        {
            try
            {
                _courtServices.CreateCourt(courtCreateDTO);
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

        // POST: api/Courts/UploadImage/{id}
        [HttpPost("UploadImage/{id}")]
        public ActionResult UploadImage(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "Invalid file" });
            }

            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "Images");
            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }

            var filePath = Path.Combine(uploads, $"{Guid.NewGuid()}_{file.FileName}");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            var relativePath = $"/Images/{Path.GetFileName(filePath)}";
            _courtServices.UpdateCourtImage(id, relativePath);

            return Ok(new { message = "Image uploaded successfully", path = relativePath });
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

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
            var result = _courtServices.GetCourts(sortContent, pageNumber, pageSize);
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

        // POST: api/Courts
        [HttpPost]
        public ActionResult CreateCourt([FromBody] CourtDTO courtDto)
        {
            _courtServices.CreateCourt(courtDto);
            return Ok(new { message = "Court created successfully" });
        }

        // PUT: api/Courts/5
        [HttpPut("{id}")]
        public ActionResult UpdateCourt(int id, [FromBody] CourtDTO courtDto)
        {
            _courtServices.UpdateCourt(id, courtDto);
            return Ok(new { message = "Court updated successfully" });
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

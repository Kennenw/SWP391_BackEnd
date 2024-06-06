using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingBad.DAL;
using BookingBad.BLL.DTO;
using BookingBad.DAL.Entities;
using BookingBad.BLL.Services;

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
            var result = _courtServices.GetCourtsPage(sortContent, pageNumber, pageSize);
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
        [HttpPost("Create")]
        public ActionResult CreateCourt([FromBody] CourtDTO courtDTO)
        {
            try
            {
                _courtServices.CreateCourt(courtDTO);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/Courts/5
        [HttpPut]
        public ActionResult UpdateCourt(int id, [FromBody] CourtDTO courtDTO)
        {
            try
            {
                _courtServices.UpdateCourt(id, courtDTO);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/Courts/5
        [HttpDelete("{id}")]
        public ActionResult DeleteCourt(int id)
        {
            try
            {
                _courtServices.DeleteCourt(id);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

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
    public class CourtNumbersController : ControllerBase
    {
        private readonly ICourtNumberServices courtNumberServices;

        public CourtNumbersController()
        {
            courtNumberServices = new CourtNumberServices();
        }

        // GET: api/CourtNumbers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourtNumberDTO>>> GetCourtNumbers()
        {
            return courtNumberServices.GetCourtNumbers();
        }

        // GET: api/CourtNumbers/Court/5
        [HttpGet("Court/{courtId:int}")]
        public async Task<ActionResult<IEnumerable<CourtNumberDTO>>> GetCourtNumbersByCourtId(int courtId)
        {
            var courtNumbers = courtNumberServices.GetCourtNumbersByCourtId(courtId);
            if (courtNumbers == null || !courtNumbers.Any())
            {
                return NotFound();
            }
            return courtNumbers;
        }

        // GET: api/CourtNumbers/5
        [HttpGet("{Number:int}")]
        public async Task<ActionResult<CourtNumberDTO>> GetCourtNumberById(int Number)
        {
            var courtNumber = courtNumberServices.GetCourtNumberById(Number);
            if (courtNumber == null)
            {
                return NotFound();
            }
            return courtNumber;
        }

        // PUT: api/CourtNumbers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourtNumber(int id, CourtNumberDTO courtNumber)
        {
            courtNumberServices.UpdateCourtNumber(id, courtNumber);

            return NoContent();
        }

        // POST: api/CourtNumbers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CourtNumberDTO>> PostCourtNumber(CourtNumberDTO courtNumber)
        {
            courtNumberServices.CreateCourtNumber(courtNumber);
            return CreatedAtAction("GetCourtNumber", new { id = courtNumber.CourtNumberId }, courtNumber);
        }

        // DELETE: api/CourtNumbers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourtNumber(int id)
        {
            var courtNumber = courtNumberServices.GetCourtNumberById(id);
            if (courtNumber == null)
            {
                return NotFound();
            }
            courtNumberServices.DeleteCourtNumber(id);
            return NoContent();
        }

    }
}

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
    public class SubCourtController : ControllerBase
    {
        private readonly ISubCourtServices subCourtServices;

        public SubCourtController()
        {
            subCourtServices = new SubCourtServices();
        }


        // PUT: api/CourtNumbers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourtNumber(int id, SubCourtDTO courtNumber)
        {
            subCourtServices.updateSubCourt(id, courtNumber);

            return NoContent();
        }

        // POST: api/CourtNumbers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SubCourtDTO>> PostCourtNumber(SubCourtDTO courtNumber)
        {
            subCourtServices.createSubCourt(courtNumber);
            return CreatedAtAction("GetSubCourt", new { id = courtNumber.SubCourtId }, courtNumber);
        }

        // DELETE: api/CourtNumbers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourtNumber(int id)
        {
            var courtNumber = subCourtServices.getSubCourt(id);
            if (courtNumber == null)
            {
                return NotFound();
            }
            subCourtServices.deleteSubCourt(id);
            return NoContent();
        }

    }
}

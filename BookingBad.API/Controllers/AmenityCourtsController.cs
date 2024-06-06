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
    public class AmenityCourtsController : ControllerBase
    {
        private readonly IAmenityCourtServices amenityCourtServices;

        public AmenityCourtsController()
        {
            amenityCourtServices = new AmenityCourtServices();
        }

        // GET: api/AmenityCourts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AmenityCourtDTO>>> GetAmenityCourts()
        {
            return amenityCourtServices.GetAmenityCourts();
        }

        // GET: api/AmenityCourts/Court/5
        [HttpGet("{courtId}")]
        public ActionResult<IEnumerable<AmenityCourtDTO>> GetAmenityByCourtId(int courtId)
        {
            var amenityCourts = amenityCourtServices.GetAmenityByCourtId(courtId);
            if (amenityCourts == null || !amenityCourts.Any())
            {
                return NotFound();
            }
            return amenityCourts;
        }

        // PUT: api/AmenityCourts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAmenityCourt(int id, AmenityCourtDTO amenityCourt)
        {
            amenityCourtServices.UpdateAmenityCourt(id, amenityCourt);
            return NoContent();
        }

        // POST: api/AmenityCourts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AmenityCourtDTO>> PostAmenityCourt(AmenityCourtDTO amenityCourt)
        {
            amenityCourtServices.CreateAmenityCourt(amenityCourt);
            return CreatedAtAction("GetAmenityCourt", new { id = amenityCourt.AmenityCourtId }, amenityCourt);
        }

        // DELETE: api/AmenityCourts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAmenityCourt(int id)
        {
            var amenityCourt = amenityCourtServices.GetAmenityCourtById(id);
            if (amenityCourt == null)
            {
                return NotFound();
            }

            amenityCourtServices.DeleteAmenityCourt(id);
            return NoContent();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
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
    public class AmenitiesController : ControllerBase
    {
        private readonly IAmenityServices amenityServices;

        public AmenitiesController()
        {
            amenityServices = new AmenityServices();
        }

        // GET: api/Amenities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AmenityDTO>>> GetAmenities()
        {
            if (amenityServices.GetAmenities() == null)
            {
                return NotFound();
            }
            return amenityServices.GetAmenities();
        }

        // GET: api/Amenities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AmenityDTO>> GetAmenity(int id)
        {
            var amenity = amenityServices.GetAmenityById(id);
            if (amenity == null)
            {
                return NotFound(new { message = "Amenity not found" });
            }
            return Ok(amenity);
        }

        // PUT: api/Amenities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAmenity(int id, AmenityDTO amenityDTO)
        {
            var amenity = amenityServices.GetAmenityById(id);
            if (amenity == null)
            {
                return BadRequest();
            }
            else
            {
                amenityServices.UpdateAmenity(id, amenityDTO);
                return Ok();
            }
        }

        // POST: api/Amenities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AmenityDTO>> PostAmenity(AmenityDTO amenityDTO)
        {
            amenityServices.CreateAmenity(amenityDTO);
            return CreatedAtAction("GetAmenity", new { id = amenityDTO.AmenityId }, amenityDTO);
        }

        // DELETE: api/Amenities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAmenity(int id)
        {
            var amenity =  this.amenityServices.GetAmenityById(id);
            if (amenity == null)
            {
                return NotFound();
            }
            amenityServices.DeleteAmenity(id);
            return NoContent();
        }
    }
}

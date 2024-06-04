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
    public class BookingsController : ControllerBase
    {
        private readonly IBookingSevices _services;

        public BookingsController()
        {
            _services = new BookingServices();
        }

        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDTO>>> GetBookings()
        {
            return _services.GetBooking();
        }

        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDTO>> GetBooking(int id)
        {
            return _services.GetBookingById(id);
        }

        // GET: api/Bookings/5
        [HttpGet("{idUser}")]
        public async Task<ActionResult<IEnumerable<BookingDTO>>> GetBookingByUser(int idUser)
        {
            var booking =  _services.GetBookingByUserId(idUser);
            if (booking == null)
            {
                return NotFound();
            }
            return booking;
        }

        // PUT: api/Bookings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(int id, BookingDTO bookingDTO)
        {
            _services.UpdateBooking(id, bookingDTO);
            return Ok();
        }

        // POST: api/Bookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Booking>> PostBooking(BookingDTO bookingDTO)
        {
            _services.CreateBooking(bookingDTO);
            return CreatedAtAction("GetBooking", new { id = bookingDTO.BookingId }, bookingDTO);
        }

        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = _services.GetBookingById(id);
            if (booking == null)
            {
                return NotFound();
            }
            _services.DeleteBooking(id);
            return NoContent();
        }
    }
}

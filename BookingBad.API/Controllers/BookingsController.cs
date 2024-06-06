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

namespace Repositories.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingSevices _bookingService;

        public BookingsController()
        {
            _bookingService = new BookingServices();
        }



        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDTO>> GetBooking(int id)
        {
            return _bookingService.GetBookingById(id);
        }

        // POST: api/Bookings/Create
        [HttpPost("Create")]
        public ActionResult CreateBooking([FromBody] BookingRequestDTO bookingRequestDTO)
        {
            try
            {
                _bookingService.CreateBooking(bookingRequestDTO);
                return Ok(new { message = "Booking created successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/Bookings/CheckIn/{bookingDetailId}
        [HttpPost("CheckIn/{bookingDetailId:int}")]
        public ActionResult CheckIn(int bookingDetailId)
        {
            try
            {
                _bookingService.CheckIn(bookingDetailId);
                return Ok(new { message = "Check-in successful" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = _bookingService.GetBookingById(id);
            if (booking == null)
            {
                return NotFound();
            }
            _bookingService.DeleteBooking(id);
            return NoContent();
        }
    }
}

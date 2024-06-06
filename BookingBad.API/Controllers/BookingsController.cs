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

        // POST: api/Bookings/Fixed
        [HttpPost("Fixed")]
        public ActionResult CreateFixedBooking([FromBody] FixedBookingDTO bookingDTO)
        {
            _bookingService.CreateFixedBooking(bookingDTO);
            return Ok(new { message = "Fixed booking created successfully" });
        }

        // POST: api/Bookings/Single
        [HttpPost("Single")]
        public ActionResult CreateSingleBooking([FromBody] SingleBookingDTO bookingDTO)
        {
            _bookingService.CreateSingleBooking(bookingDTO);
            return Ok(new { message = "Single booking created successfully" });
        }

        // POST: api/Bookings/Flexible
        [HttpPost("Flexible")]
        public ActionResult CreateFlexibleBooking([FromBody] FlexibleBookingDTO bookingDTO)
        {
            _bookingService.CreateFlexibleBooking(bookingDTO);
            return Ok(new { message = "Flexible booking created successfully" });
        }

        // POST: api/Bookings/CheckIn/{bookingDetailId}
        [HttpPost("CheckIn/{bookingDetailId:int}")]
        public ActionResult CheckIn(int bookingDetailId)
        {
            _bookingService.CheckIn(bookingDetailId);
            return Ok(new { message = "Check-in successful" });
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

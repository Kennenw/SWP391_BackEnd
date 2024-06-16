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

        public BookingsController(IBookingSevices bookingService)
        {
            _bookingService = bookingService;
        }
        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDTO>> GetBooking(int id)
        {
            return _bookingService.GetBookingById(id);
        }

        // POST: api/Schedules/Fixed
        [HttpPost("Fixed")]
        public async Task<IActionResult> BookFixedSchedule([FromBody] FixedScheduleDTO scheduleDTO)
        {
            var result = await _bookingService.BookFixedSchedule(scheduleDTO);
            return Ok(result);
        }

        // POST: api/Schedules/OneTime
        [HttpPost("OneTime")]
        public async Task<IActionResult> BookOneTimeSchedule([FromBody] OneTimeScheduleDTO scheduleDTO)
        {
            var result = await _bookingService.BookOneTimeSchedule(scheduleDTO);
            return Ok(result);
        }

        // POST: api/Schedules/Flexible
        [HttpPost("Flexible")]
        public async Task<IActionResult> BookFlexibleSchedule([FromBody] FlexibleScheduleDTO scheduleDTO)
        {
            var result = await _bookingService.BookFlexibleSchedule(scheduleDTO);
            return Ok(result);
        }

        // POST: api/Schedules/FlexibleSlot
        [HttpPost("FlexibleSlot")]
        public async Task<IActionResult> BookFlexibleSlot([FromBody] BookedSlotDTO bookedSlotDTO)
        {
            var result = await _bookingService.BookFlexibleSlot(bookedSlotDTO);
            return Ok(result);
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

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDTO>>> GetBooking()
        {
            return _bookingService.GetBooking();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDTO>> GetBookingById(int id)
        { 
            var item = _bookingService.GetBookingById(id);
            if(item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpGet("ByCustomer/{customerId}")]
        public IActionResult GetBookingsByCustomerId(int customerId)
        {
            try
            {
                var bookings = _bookingService.GetBookingsByCustomerId(customerId);
                if (bookings == null || bookings.Count == 0)
                {
                    return NotFound(new { message = "No bookings found for the given customer ID" });
                }
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("Fixed")]
        public async Task<IActionResult> BookFixedSchedule([FromBody] FixedScheduleDTO scheduleDTO)
        {
            var result = await _bookingService.BookFixedSchedule(scheduleDTO);
            return CreatedAtAction(nameof(GetBookingById), new { id = result.BookingId }, result);
        }

        [HttpPost("OneTime")]
        public async Task<IActionResult> BookOneTimeSchedule([FromBody] OneTimeScheduleDTO scheduleDTO)
        {
            var result = await _bookingService.BookOneTimeSchedule(scheduleDTO);
            return CreatedAtAction(nameof(GetBookingById), new { id = result.BookingId }, result);
        }

        [HttpPost("Flexible")]
        public async Task<IActionResult> BookFlexibleSchedule([FromBody] FlexibleScheduleDTO scheduleDTO)
        {
            var result = await _bookingService.BookFlexibleSchedule(scheduleDTO);
            return CreatedAtAction(nameof(GetBookingById), new { id = result.BookingId }, result);
        }

        [HttpPost("FlexibleSlot")]
        public async Task<IActionResult> BookFlexibleSlot([FromBody] BookedSlotDTO bookedSlotDTO)
        {
            try
            {
                var result = await _bookingService.BookFlexibleSlot(bookedSlotDTO);
                return CreatedAtAction(nameof(GetBookingById), new { id = result.BookingId }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("CheckIn")]
        public async Task<IActionResult> CheckIn([FromBody] CheckInDTO request)
        {
            try
            {
                var response = await _bookingService.CheckIn(request.SubCourtId, request.BookingDetailId);
                if (response != null)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(new { message = "Invalid court ID or booking detail ID" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


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
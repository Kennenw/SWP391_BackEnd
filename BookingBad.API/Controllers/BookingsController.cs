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
    public class BookingsController : ControllerBase
    {
        private readonly IBookingSevices _bookingService;

        public BookingsController()
        {
            _bookingService = new BookingServices();
        }

        // GET: api/Bookings
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<BookingDTO>>> GetBooking(
            [FromQuery] SortBookingByEnum sortBookingBy,
            [FromQuery] SortTypeEnum sortBookingType,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var sortContent = new SortContent
            {
                sortBookingBy = sortBookingBy,
                sortType = sortBookingType,
            };
            var result = _bookingService.GetBooking(sortContent, pageNumber, pageSize);
            if (result == null)
            {
                return NotFound(new { message = "Is empty" });
            }
            return Ok(result);
        }

        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDTO>> GetBooking(int id)
        {
            return _bookingService.GetBookingById(id);
        }


        // POST: api/Bookings/Fixed
        [HttpPost("Fixed")]
        public async Task<ActionResult> CreateFixedSchedule([FromBody] FlexibleScheduleRequest request)
        {
            try
            {
                await Task.Run(() => _bookingService.CreateFixedSchedule(request.BookingF, request.ScheduleF));
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/Bookings/SingleDay
        [HttpPost("SingleDay")]
        public async Task<ActionResult> CreateSingleDayBooking([FromBody] SingleDayBookingRequest request)
        {
            try
            {
                await Task.Run(() => _bookingService.CreateSingleDayBooking(request.BookingS, request.BookingDetailS));
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/Bookings/Flexible
        [HttpPost("Flexible")]
        public async Task<ActionResult> CreateFlexibleSchedule([FromBody] FlexibleScheduleRequest request)
        {
            try
            {
                await Task.Run(() => _bookingService.CreateFlexibleSchedule(request.BookingF, request.ScheduleF));
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/Bookings/CheckIn/{bookingDetailID}
        [HttpPost("CheckIn/{bookingDetailID}")]
        public async Task<ActionResult> CheckIn(int bookingDetailID)
        {
            try
            {
                _bookingService.CheckIn(bookingDetailID);
                return Ok();
            }
            catch (InvalidOperationException ex)
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

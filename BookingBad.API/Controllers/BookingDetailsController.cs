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
    public class BookingDetailsController : ControllerBase
    {
        private readonly IBookingDetailsServices _bookingDetailsServices;

        public BookingDetailsController(IBookingDetailsServices bookingDetailsServices)
        {
            _bookingDetailsServices = bookingDetailsServices;
        }

        // GET: api/BookingDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDetailDTO>>> GetBookingDetails()
        {
            return _bookingDetailsServices.GetBookingDetails();
        }

        // GET: api/BookingDetails/5
        [HttpGet("{bookingId}")]
        public IActionResult GetBookingDetailsByBookingId(int bookingId)
        {
            try
            {
                var bookingDetails = _bookingDetailsServices.GetBookingDetailsByBookingId(bookingId);
                if (bookingDetails == null || bookingDetails.Count == 0)
                {
                    return NotFound(new { message = "No booking details found for the given booking ID" });
                }
                return Ok(bookingDetails);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}

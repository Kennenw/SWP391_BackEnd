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
    public class BookingTypesController : ControllerBase
    {
        private readonly IBookingTypeServices _bookingTypeService;

        public BookingTypesController()
        {
            _bookingTypeService = new BookingTypeServices();
        }

        // GET: api/BookingTypes
        [HttpGet]
        public ActionResult<IEnumerable<BookingTypeDTO>> GetBookingTypes()
        {
            var bookingTypes = _bookingTypeService.GetAllBookingTypes();
            return Ok(bookingTypes);
        }

        // GET: api/BookingTypes/5
        [HttpGet("{id}")]
        public ActionResult<BookingTypeDTO> GetBookingTypeById(int id)
        {
            var bookingType = _bookingTypeService.GetBookingTypeById(id);
            if (bookingType == null)
            {
                return NotFound();
            }
            return Ok(bookingType);
        }

        // POST: api/BookingTypes
        [HttpPost]
        public ActionResult CreateBookingType([FromBody] BookingTypeDTO bookingTypeDTO)
        {
            _bookingTypeService.CreateBookingType(bookingTypeDTO);
            return Ok(new { message = "Booking type created successfully" });
        }

        // PUT: api/BookingTypes/5
        [HttpPut("{id}")]
        public ActionResult UpdateBookingType(int id, [FromBody] BookingTypeDTO bookingTypeDTO)
        {
            _bookingTypeService.UpdateBookingType(id, bookingTypeDTO);
            return Ok(new { message = "Booking type updated successfully" });
        }

        // DELETE: api/BookingTypes/5
        [HttpDelete("{id}")]
        public ActionResult DeleteBookingType(int id)
        {
            _bookingTypeService.DeleteBookingType(id);
            return Ok(new { message = "Booking type deleted successfully" });
        }
    }
}

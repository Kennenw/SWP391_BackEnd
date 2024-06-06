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
    public class SlotTimesController : ControllerBase
    {
        private readonly ISlotTimeServices timeServices;

        public SlotTimesController()
        {
            timeServices = new SlotTimeServices();
        }

        // GET: api/SlotTimes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SlotTimeDTO>>> GetSlotTimes()
        {
            return timeServices.GetSlot();
        }

        // GET: api/SlotTimes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SlotTimeDTO>> GetSlotTime(int id)
        {
            var slotTime = timeServices.GetSlotById(id);
            if (slotTime == null)
            {
                return NotFound();
            }
            return slotTime;
        }

        // PUT: api/SlotTimes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSlotTime(int id, SlotTimeDTO slotTimeDTO)
        {
            timeServices.UpdateSlot(id, slotTimeDTO);
            return NoContent();
        }

        // POST: api/SlotTimes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SlotTimeDTO>> PostSlotTime(SlotTimeDTO slotTimeDTO)
        {
            timeServices.CreateSlot(slotTimeDTO);
            return CreatedAtAction("GetSlotTime", new { id = slotTimeDTO.SlotId }, slotTimeDTO);
        }

        // DELETE: api/SlotTimes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSlotTime(int id)
        {
            var slotTime = timeServices.GetSlotById(id);
            if (slotTime == null)
            {
                return NotFound();
            }
            timeServices.DeleteSlot(id);
            return NoContent();
        }
    }
}

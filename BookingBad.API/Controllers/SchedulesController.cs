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
    public class SchedulesController : ControllerBase
    {
        private readonly IScheduleServices scheduleServices;

        public SchedulesController()
        {
            scheduleServices = new ScheduleServices();
        }

        // GET: api/Schedules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScheduleDTO>>> GetSchedules()
        {
            return scheduleServices.GetSchedule();
        }
        // PUT: api/Schedules/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSchedule(int id, ScheduleDTO scheduleDTO)
        {
            try
            {
                scheduleServices.UpdateSchedule(id, scheduleDTO);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        // GET: api/Schedules/5
        [HttpGet("{id:int}")]
        public ActionResult<ScheduleDTO> GetSchedule(int id)
        {
            var schedule = scheduleServices.GetScheduleById(id);
            if (schedule == null)
            {
                return NotFound();
            }
            return Ok(schedule);
        }

        // POST: api/Schedules
        [HttpPost]
        public async Task<ActionResult<ScheduleDTO>> PostSchedule(ScheduleDTO scheduleDTO)
        {
            try
            {
                scheduleServices.CreateSchedule(scheduleDTO);
                return CreatedAtAction("GetSchedule", new { id = scheduleDTO.ScheduleId }, scheduleDTO);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        // DELETE: api/Schedules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var schedule = scheduleServices.GetScheduleById(id);
            if (schedule == null)
            {
                return NotFound();
            }
            scheduleServices.DeleteSchedule(id);
            return NoContent();
        }
    }
}

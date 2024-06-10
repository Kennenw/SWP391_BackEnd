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
    public class AreasController : ControllerBase
    {
        private readonly IAreaServices areaServices;

        public AreasController()
        {
            areaServices = new AreaServices();
        }

        // GET: api/Areas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AreaDTO>>> GetArea()
        {
            return areaServices.GetArea();
        }

        // GET: api/Areas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AreaDTO>> GetArea(int id)
        {
            var area = areaServices.GetAreaById(id);

            if (area == null)
            {
                return NotFound();
            }

            return area;
        }

        // PUT: api/Areas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArea(int id, AreaDTO area)
        {
            areaServices.UpdateArea(id, area);
            return Ok(new { message = "Area update successfully" });
        }

        // POST: api/Areas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AreaDTO>> PostArea(AreaDTO area)
        {
            areaServices.CreateArea(area);
            return CreatedAtAction("GetArea", new { id = area.AreaId }, area);
        }

        // DELETE: api/Areas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArea(int id)
        {
            var area = areaServices.GetAreaById(id);
            if (area == null)
            {
                return NotFound();
            }
            areaServices.DeleteArea(id);
            return NoContent();
        }
    }
}

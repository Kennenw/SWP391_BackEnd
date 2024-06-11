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
using Repositories.Repositories;
using Services;

namespace BookingDemo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleServices roleSrvices;

        public RolesController()
        {
            roleSrvices = new RoleServices();
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDTO>>> GetRoles()
        {
            if(roleSrvices.GetRole() == null)
            {
                return NotFound();
            }
            return roleSrvices.GetRole();
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDTO>> GetRole(int id)
        {
            var role =  roleSrvices.GetRoleById(id);

            if (role == null)
            {
                return NotFound();
            }

            return role;
        }


        [HttpPut("id")]
        public async Task<IActionResult> UpdateRole( int role_id, RoleDTO roleDTO)
        {
            roleSrvices.UpdateRole(role_id, roleDTO);  
            return Ok();
        }

        // POST: api/Roles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RoleDTO>> CreateRole(RoleDTO role)
        {
            roleSrvices.CreateRole(role);
            return CreatedAtAction("GetRole", new { id = role.RoleId }, role);
        }


        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role =  this.roleSrvices.GetRoleById(id);
            if (role == null)
            {
                return NotFound();
            }
            roleSrvices.DeleteRole(id);
            return NoContent();
        }

    }
}

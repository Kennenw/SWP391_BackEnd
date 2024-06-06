using System;
using System.Collections.Generic;
using System.Data;
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
    public class AccountsController : ControllerBase
    {
        private readonly IAccountServices  accountServices;

        public AccountsController()
        {
            accountServices = new AccountServices();
        }

        // GET: api/Accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> GetAccounts(
            [FromQuery] SortAccountByEnum sortAccountBy,
            [FromQuery] SortTypeEnum sortAccountType,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var sortContent = new SortContent
            {
                sortAccountBy = sortAccountBy,
                sortType = sortAccountType,
            };
            var result = accountServices.GetAccount(sortContent, pageNumber, pageSize);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpGet("Search")]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> SearchAccount(
            [FromQuery] string searchTerm, 
            [FromQuery] SortAccountByEnum sortAccountBy, 
            [FromQuery] SortTypeEnum sortAccountType,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var sortContent = new SortContent{
                sortAccountBy = sortAccountBy,
                sortType = sortAccountType,
            };
            var result = accountServices.PagedResult(searchTerm ,sortContent ,pageNumber ,pageSize );
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }



        // GET: api/Accounts/5
        [HttpGet("id")]
        public async Task<ActionResult<AccountDTO>> GetAccount(int id)
        {
            var account =  accountServices.GetAccountById(id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }


        [HttpGet("Login")]
        public async Task<ActionResult<AccountDTO>> Login(string email, string pass)
        {
            var account = accountServices.Login(email,pass);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }
        // PUT: api/Accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("id")]
        public async Task<IActionResult> PutAccount(int id, AccountDTO accountDTO)
        {
            accountServices.UpdateAccount(id, accountDTO);
            return NoContent();
        }

        // POST: api/Accounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Account>> PostAccount(AccountDTO account)
        {
            accountServices.CreateAccount(account);
            return CreatedAtAction("GetAccount", new { id = account.AccountId }, account);
        }

        // DELETE: api/Accounts/5
        [HttpDelete("id")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = this.accountServices.GetAccountById(id);
            if (account == null)
            {
                return NotFound();
            }
            accountServices.DeleteAccount(id);
            return NoContent();
        }

    }
}

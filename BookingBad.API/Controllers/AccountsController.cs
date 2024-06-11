﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
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

        [HttpGet("SelfProfile/{id:int}")]
        public async Task<ActionResult<SelfProfile>> SeltProfile(int id)
        {
            var account = accountServices.GetSelfProfile(id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

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
       

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterInformation info)
        {
            if (info.Password != info.ReEnterPass)
            {
                return Ok(new SuccessObject<object> { Message = "Mật khẩu không trùng khớp" });
            }
            if (accountServices.IsUserExist(info.Email))
            {
                return Ok(new SuccessObject<object> { Message = "Email này đã tồn tại" });
            }
            var account = accountServices.RegisterUser(info);
            return Ok(account);
        }

        [HttpPut("UpdatePass/{email}")]
        public async Task<IActionResult> UpdatePass(string email, UpdatePassword info)
        {
            if (!accountServices.IsUserExist(email))
            {
                return Ok(new SuccessObject<object> { Message = "Không tìm thấy người dùng !" });
            }

            if (info.NewPassword != info.ReEnterPassword)
            {
                return Ok(new SuccessObject<object> { Message = "Xác minh mật khẩu không khớp !" });
            }
            var account = accountServices.UpdatePassword(email, info);
            return Ok(account);
        }

        [HttpPut("UpdateProfile/{user_id}")]
        public async Task<IActionResult> UpdateProfile(int user_id, UpdateProfileUser info)
        {
            try
            {
                var data = accountServices.UpdateProfile(user_id, info);
                return Ok(new SuccessObject<object> { Data = data, Message = "Update profile sussecfully." });
            }
            catch
            {
                return Ok(new SuccessObject<object> { Message = "Invalid base 64 string" });
            }
        }

        [HttpPut("{user_id}/setting_password")]
        public async Task<IActionResult> SettingPassword(int user_id, SettingPasswordRequest info)
        {
            var res = await accountServices.SettingPassword(user_id, info);
            if (res == 1)
            {
                return Ok(new SuccessObject<object?> { Data = true, Message = $"Cập nhật thành công!" });
            }
            else if (res == 0)
            {
                return Ok(new SuccessObject<object?> { Message = "Mật khẩu cũ không hợp lệ !" });
            }
            else if (res == -1)
            {
                return Ok(new SuccessObject<object?> { Message = "Nhập lại mật khẩu sai !" });
            }
            else if (res == -2)
            {
                return Ok(new SuccessObject<object?> { Message = "Người dùng không tồn tại !" });
            }
            else
            {
                return Ok(new SuccessObject<object?> { Message = "Cập nhật thất bại" });
            }
        }

        [HttpPut("{user_id}/to/{role_id}/by/{admin_id}")]
        public async Task<IActionResult> UpdateRoleUser(int user_id, Role role_id, int admin_id)
        {
            try
            {
                if (!accountServices.IsAdmin(admin_id))
                    throw new Exception("Bạn không có quyền truy cập !");

                if (accountServices.UpdateRoleUser(user_id, role_id))
                    return Ok(new SuccessObject<object> { Data = true, Message = "Cập nhật thành công" });
                else
                {
                    throw new Exception("Cập nhật thất bại !");
                }
            }
            catch (Exception ex)
            {
                return Ok(new SuccessObject<object> { Message = ex.Message });
            }
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            if (!accountServices.IsAdmin(id))
                return Ok(new SuccessObject<object> { Message = "Bạn không có quyền truy cập !" });
            accountServices.DeleteAccount(id);
            return Ok(new SuccessObject<object> { Message = "Xóa thành công" });                          
        }
    }
}

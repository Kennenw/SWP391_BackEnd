﻿using Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class AccountDTO
    {
        public int AccountId { get; set; }

        public string? AccountName { get; set; }

        public string? Password { get; set; }

        public string? FullName { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }
        public string? Image {  get; set; }
        public int? RoleId { get; set; }
        public double? Balance { get; set; } = 0;
        public bool? Status { get; set; }
    }


    public class RegisterInformation //request
    {
        public string? FullName { get; set; }
        public string? PhoneNum { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ReEnterPass { get; set; }
        public string? UserName { get; set; }
    }
    public class UpdatePassword //request
    {
        public string NewPassword { get; set; }
        public string ReEnterPassword { get; set; }
    }
    public class UpdateProfileUser //request
    {
        public string UserName { get; set; } = null!;
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ImgUrl { get; set; }
    }
    public class SettingPasswordRequest //request
    {
        public string? OldPass { get; set; }
        public string? NewPass { get; set; }
        public string? ReEnterPass { get; set; }
    }
    public class SelfProfile //response
    {
        public string UserName { get; set; } = null!;
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ImgUrl { get; set; }
        public double? Balance { get; set; } = 0;
    }
    
}

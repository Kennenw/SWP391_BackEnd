using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBad.BLL.DTO
{
    public class AccountDTO
    {
        public int AccountId { get; set; }

        public string? AccountName { get; set; }

        public string? Password { get; set; }

        public string? FullName { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public int? RoleId { get; set; }

        public bool? Status { get; set; }

    }
}

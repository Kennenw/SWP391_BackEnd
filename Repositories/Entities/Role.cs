using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingBad.DAL.Entities;

public partial class Role
{
    public int RoleId { get; set; }
    public string? RoleName { get; set; }
    public bool? Status { get; set; }
    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
    [NotMapped] //không lưu vào database
    public List<Role> RoleCollection { get; set; }
}

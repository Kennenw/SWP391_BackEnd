using System;
using System.Collections.Generic;

namespace Repositories.Entities;

public partial class Account
{
    public int AccountId { get; set; }

    public string? AccountName { get; set; }

    public string? Password { get; set; }

    public string? FullName { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public int? RoleId { get; set; }

    public bool? Status { get; set; }

    public string? Image { get; set; }
    public double? Balance { get; set; } = 0;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Court> Courts { get; set; } = new List<Court>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual ICollection<RatingCourt> RatingCourts { get; set; } = new List<RatingCourt>();

    public virtual ICollection<RatingPost> RatingPosts { get; set; } = new List<RatingPost>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<SlotTime> SlotTimes { get; set; } = new List<SlotTime>();
}

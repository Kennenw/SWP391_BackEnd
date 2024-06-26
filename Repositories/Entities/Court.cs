﻿using System;
using System.Collections.Generic;

namespace Repositories.Entities;

public partial class Court
{
    public int CourtId { get; set; }

    public int? AreaId { get; set; }

    public string? CourtName { get; set; }

    public string? OpenTime { get; set; }

    public string? CloseTime { get; set; }

    public string? Rules { get; set; }

    public bool? Status { get; set; }

    public string? Image { get; set; }

    public int? ManagerId { get; set; }

    public string? Title { get; set; }

    public string? Address { get; set; }

    public double? TotalRate { get; set; }

    public double? Rate { get; set; }

    public double? PricePerHour { get; set; }

    public virtual ICollection<AmenityCourt> AmenityCourts { get; set; } = new List<AmenityCourt>();

    public virtual Area? Area { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Account? Manager { get; set; }

    public virtual ICollection<RatingCourt> RatingCourts { get; set; } = new List<RatingCourt>();

    public virtual ICollection<SlotTime> SlotTimes { get; set; } = new List<SlotTime>();

    public virtual ICollection<SubCourt> SubCourts { get; set; } = new List<SubCourt>();
}

﻿using System;
using System.Collections.Generic;

namespace ClassLibrary1.Entities;

public partial class BookingType
{
    public int BookingTypeId { get; set; }

    public string? Description { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}

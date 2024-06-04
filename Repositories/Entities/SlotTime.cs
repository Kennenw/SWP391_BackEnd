using System;
using System.Collections.Generic;

namespace Repositories.Entities;

public partial class SlotTime
{
    public int SlotId { get; set; }

    public string? StartTime { get; set; }

    public string? EndTime { get; set; }

    public double? Price { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}

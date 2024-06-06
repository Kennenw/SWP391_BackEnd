using System;
using System.Collections.Generic;

namespace Repositories.Entities;

public partial class Schedule
{
    public int ScheduleId { get; set; }

    public int? CourtNumberId { get; set; }

    public int? SlotId { get; set; }

    public int? BookingTypeId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? TotalHours { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();

    public virtual BookingType? BookingType { get; set; }

    public virtual CourtNumber? CourtNumber { get; set; }

    public virtual SlotTime? Slot { get; set; }
}

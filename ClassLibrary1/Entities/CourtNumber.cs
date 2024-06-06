using System;
using System.Collections.Generic;

namespace ClassLibrary1.Entities;

public partial class CourtNumber
{
    public int CourtNumberId { get; set; }

    public int? Number { get; set; }

    public int? CourtId { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();

    public virtual Court? Court { get; set; }

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}

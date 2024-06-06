using System;
using System.Collections.Generic;

namespace BookingBad.DAL.Entities;

public partial class CourtNumber
{
    public int CourtNumberId { get; set; }

    public int? Number { get; set; }

    public int? CourtId { get; set; }

    public bool? Status { get; set; }

    public virtual Court? Court { get; set; }

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}

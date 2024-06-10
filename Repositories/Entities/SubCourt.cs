using System;
using System.Collections.Generic;

namespace Repositories.Entities;

public partial class SubCourt
{
    public int SubCourtId { get; set; }

    public int? Number { get; set; }

    public int? CourtId { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();

    public virtual Court? Court { get; set; }

    public virtual ICollection<SlotTime> SlotTimes { get; set; } = new List<SlotTime>();
}

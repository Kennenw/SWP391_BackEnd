using System;
using System.Collections.Generic;

namespace ClassLibrary1.Entities;

public partial class BookingDetail
{
    public int BookingDetailId { get; set; }

    public int? BookingId { get; set; }

    public int? SlotId { get; set; }

    public DateTime? Date { get; set; }

    public bool? Status { get; set; }

    public int? ScheludeId { get; set; }

    public bool? CheckIn { get; set; }

    public int? CourtNumberId { get; set; }

    public virtual Booking? Booking { get; set; }

    public virtual CourtNumber? CourtNumber { get; set; }

    public virtual Schedule? Schelude { get; set; }

    public virtual SlotTime? Slot { get; set; }
}

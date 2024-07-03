using System;
using System.Collections.Generic;

namespace Repositories.Entities;

public partial class Booking
{
    public int BookingId { get; set; }

    public int? CustomerId { get; set; }

    public int? BookingTypeId { get; set; }

    public int? PlayerQuantity { get; set; } = 1;

    public double? TotalPrice { get; set; } = 0;

    public string? Note { get; set; } = string.Empty;

    public bool? Status { get; set; }

    public double? TotalHours { get; set; } = 0;

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? CourtId { get; set; }

    public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();

    public virtual BookingType? BookingType { get; set; }

    public virtual Court? Court { get; set; }

    public virtual Account? Customer { get; set; }

    public virtual ICollection<Payments> Payments { get; set; } = new List<Payments>();
}

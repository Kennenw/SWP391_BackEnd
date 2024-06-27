using System;
using System.Collections.Generic;

namespace Repositories.Entities;

public partial class Payments
{
    public int PaymentId { get; set; }

    public int? BookingId { get; set; }

    public DateTime? PaymentDate { get; set; }

    public double? PaymentAmount { get; set; }

    public double? TotalAmount { get; set; }

    public virtual Booking? Booking { get; set; }
}

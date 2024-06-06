﻿using System;
using System.Collections.Generic;

namespace BookingBad.DAL.Entities;

public partial class Booking
{
    public int BookingId { get; set; }

    public int? CustomerId { get; set; }

    public int? BookingTypeId { get; set; }

    public int? PlayerQuantity { get; set; }

    public double? TotalPrice { get; set; }

    public string? Note { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<BookingDetail> BookingDetails { get; set; } = new List<BookingDetail>();

    public virtual BookingType? BookingType { get; set; }

    public virtual Account? Customer { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}

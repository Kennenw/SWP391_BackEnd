﻿using System;
using System.Collections.Generic;

namespace Repositories.Entities;

public partial class BookingDetail
{
    public int BookingDetailId { get; set; }

    public int? BookingId { get; set; }

    public int? SlotId { get; set; }

    public DateTime? Date { get; set; }

    public bool? Status { get; set; }

    public int? SubCourtId { get; set; }

    public virtual Booking? Booking { get; set; }

    public virtual SlotTime? Slot { get; set; }

    public virtual SubCourt? SubCourt { get; set; }
}

using System;
using System.Collections.Generic;

namespace Repositories.Entities;

public partial class BookingType
{
    public int BookingTypeId { get; set; }

    public string? Description { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}

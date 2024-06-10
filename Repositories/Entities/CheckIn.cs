using System;
using System.Collections.Generic;

namespace Repositories.Entities;

public partial class CheckIn
{
    public int CheckInId { get; set; }

    public int? BookingDetailId { get; set; }

    public DateTime? CheckInTime { get; set; }

    public virtual BookingDetail? BookingDetail { get; set; }
}

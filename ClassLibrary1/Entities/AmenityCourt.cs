using System;
using System.Collections.Generic;

namespace ClassLibrary1.Entities;

public partial class AmenityCourt
{
    public int AmenityCourtId { get; set; }

    public int? AmenityId { get; set; }

    public int? CourtId { get; set; }

    public bool? Status { get; set; }

    public virtual Amenity? Amenity { get; set; }

    public virtual Court? Court { get; set; }
}

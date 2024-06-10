using System;
using System.Collections.Generic;

namespace Repositories.Entities;

public partial class Amenity
{
    public int AmenitiId { get; set; }

    public string? Description { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<AmenityCourt> AmenityCourts { get; set; } = new List<AmenityCourt>();
}

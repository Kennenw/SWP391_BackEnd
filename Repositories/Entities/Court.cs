﻿using System;
using System.Collections.Generic;

namespace Repositories.Entities;

public partial class Court
{
    public int CourtId { get; set; }

    public int? AreaId { get; set; }

    public string? CourtName { get; set; }

    public string? OpenTime { get; set; }

    public string? CloseTime { get; set; }

    public string? Rule { get; set; }

    public bool? Status { get; set; }

    public string? Image { get; set; }

    public virtual Area? Area { get; set; }

    public virtual ICollection<AmenityCourt> AmenityCourts { get; set; } = new List<AmenityCourt>();

    public virtual ICollection<CourtNumber> CourtNumbers { get; set; } = new List<CourtNumber>();
}

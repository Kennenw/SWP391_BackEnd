using System;
using System.Collections.Generic;

namespace Repositories.Entities;

public partial class Area
{
    public int AreaId { get; set; }

    public string? Location { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<Court> Courts { get; set; } = new List<Court>();
}

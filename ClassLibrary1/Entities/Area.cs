using System;
using System.Collections.Generic;

namespace ClassLibrary1.Entities;

public partial class Area
{
    public int AreaId { get; set; }

    public int? ManagerId { get; set; }

    public string? Location { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<Court> Courts { get; set; } = new List<Court>();

    public virtual Account? Manager { get; set; }
}

﻿using System;
using System.Collections.Generic;

namespace Repositories.Entities;

public partial class Post
{
    public int PostId { get; set; }

    public int? AccountId { get; set; }

    public string? Content { get; set; }

    public double? TotalRate { get; set; }

    public bool? Status { get; set; }

    public double? Rate { get; set; }

    public string? Image { get; set; }

    public string? Title { get; set; }

    public virtual Account? Account { get; set; }
}

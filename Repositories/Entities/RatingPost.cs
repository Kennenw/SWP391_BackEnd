using System;
using System.Collections.Generic;

namespace Repositories.Entities;

public partial class RatingPost
{
    public int RatingId { get; set; }

    public int? PostId { get; set; }

    public int? UserId { get; set; }

    public double? RatingValue { get; set; }

    public virtual Post? Post { get; set; }

    public virtual Account? User { get; set; }
}

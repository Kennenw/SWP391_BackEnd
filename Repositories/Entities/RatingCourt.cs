using System;
using System.Collections.Generic;

namespace Repositories.Entities;

public partial class RatingCourt
{
    public int RatingCourtId { get; set; }

    public int? CourtId { get; set; }

    public int? UserId { get; set; }

    public double? RatingValue { get; set; }

    public virtual Court? Court { get; set; }

    public virtual Account? User { get; set; }
}

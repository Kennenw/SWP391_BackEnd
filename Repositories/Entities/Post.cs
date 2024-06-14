using System;
using System.Collections.Generic;

namespace Repositories.Entities;

public partial class Post
{
    public int PostId { get; set; }

    public int? AccountId { get; set; }

    public string? Context { get; set; }

    public double? TotalRate { get; set; } = 0;

    public bool? Status { get; set; }

    public string? Image { get; set; }

    public string? Title { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<RatingPost> RatingPosts { get; set; } = new List<RatingPost>();
}

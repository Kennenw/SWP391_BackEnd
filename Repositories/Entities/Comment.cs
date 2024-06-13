using System;
using System.Collections.Generic;

namespace Repositories.Entities;

public partial class Comment
{
    public int CommentId { get; set; }

    public string? Title { get; set; }

    public string? Context { get; set; }

    public int? UserId { get; set; }

    public bool? Status { get; set; }

    public string? Image { get; set; }
}

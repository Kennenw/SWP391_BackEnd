using System;
using System.Collections.Generic;

namespace ClassLibrary1.Entities;

public partial class Comment
{
    public int CommentId { get; set; }

    public string? Title { get; set; }

    public byte[]? Image { get; set; }

    public string? Context { get; set; }

    public int? PostId { get; set; }

    public bool? Status { get; set; }

    public virtual Post? Post { get; set; }
}

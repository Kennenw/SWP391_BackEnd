using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class PostDTO
    {
        public int PostId { get; set; }
        public int? AccountId { get; set; }
        public string? Context { get; set; }
        public double? TotalRate { get; set; } = 0;
        public string? Image { get; set; }
        public string? Title { get; set; }

    }
    public class CommentDTO
    {
        public int CommentId { get; set; }
        public string? Title { get; set; }
        public string? Context { get; set; }
        public int? UserId { get; set; }
        public bool? Status { get; set; }
        public string? Image { get; set; }
        public int? PostId { get; set; }
        public int? CourtId { get; set; }
    }

    public class RatingPostDTO
    {
        public int UserId { get; set; }
        public double RatingValue { get; set; }
    }
}

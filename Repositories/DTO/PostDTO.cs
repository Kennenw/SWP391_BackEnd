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

        public byte[]? Image { get; set; }

        public string? Context { get; set; }

        public string? Vote { get; set; }

        public List<CommentDTO> Comments { get; set; } = new List<CommentDTO>();
    }

    public class CommentDTO
    {
        public int CommentId { get; set; }

        public string? Title { get; set; }

        public byte[]? Image { get; set; }

        public string? Context { get; set; }

        public int? PostId { get; set; }
    }
}

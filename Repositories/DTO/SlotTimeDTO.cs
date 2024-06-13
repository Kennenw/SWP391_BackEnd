using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class SlotTimeDTO
    {
        public int SlotId { get; set; }

        public string? StartTime { get; set; }

        public string? EndTime { get; set; }

        public double? Price { get; set; }
        public int? ManagerId { get; set; }

        public bool? Status { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
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
        public double? WeekdayPrice { get; set; }
        public double? WeekendPrice { get; set; }
        public bool? Status { get; set; }
        public int? ManagerId { get; set; }
        public int? SubCourtId { get; set; }
        public int? CourtId { get; set; }
    }
}

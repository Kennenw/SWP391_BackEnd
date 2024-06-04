using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class ScheduleDTO
    {
        public int ScheduleId { get; set; }

        public int? CourtNumberId { get; set; }

        public int? SlotId { get; set; }

        public int? BookingTypeId { get; set; }

        public DateOnly? StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

        public int? TotalHours { get; set; }
    }
}

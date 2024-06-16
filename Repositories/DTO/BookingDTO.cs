using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class BookingDTO
    {
        public int BookingId { get; set; }

        public int? CustomerId { get; set; }

        public int? BookingTypeId { get; set; }

        public int? PlayerQuantity { get; set; }

        public double? TotalPrice { get; set; }

        public string? Note { get; set; }

        public bool? Status { get; set; }
    }
    public class FixedScheduleDTO
    {
        public int CourtId { get; set; }
        public int SubCourtId { get; set; }
        public int UserId { get; set; }
        public string DayOfWeek { get; set; } 
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int Months { get; set; } 
    }

    public class OneTimeScheduleDTO
    {
        public int CourtId { get; set; }
        public int SubCourtId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }

    public class FlexibleScheduleDTO
    {
        public int CourtId { get; set; }
        public int SubCourtId { get; set; }
        public int UserId { get; set; }
        public int TotalHours { get; set; } 
    }

    public class BookedSlotDTO
    {
        public int ScheduleId { get; set; }
        public DateTime Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }

}

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
        public int UserId { get; set; }
        public int SlotTimeId { get; set; }
        public int Months { get; set; } 
        public string Note {  get; set; }
        public DateTime Date { get; set; }

    }

    public class OneTimeScheduleDTO
    {
        public int UserId { get; set; }
        public int PlayerQuantity { get; set; } = 1;
        public int TotalPrice { get; set; } = 0;
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public int SlotTimeId { get; set; }
    }

    public class FlexibleScheduleDTO
    {
        public int CourtId { get; set; }
        public int SubCourtId { get; set; }
        public int UserId { get; set; }
        public double TotalHours { get; set; } // Tổng số giờ chơi trong 1 tháng
    }

    public class BookedSlotDTO
    {
        public int BookingId { get; set; }
        public DateTime Date { get; set; }
        public int SlotTimeId { get; set; } // Tham chiếu đến SlotTime
    }
}



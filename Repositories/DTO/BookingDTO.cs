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
    public class BookingRequestDTO
    {
        public int CustomerId { get; set; }
        public int BookingTypeId { get; set; }
        public int SubCourtId { get; set; }
        public int SlotId { get; set; }
        public DateTime Date { get; set; }
        public int PlayerQuantity { get; set; }
        public string Note { get; set; }
        public int MonthsDuration { get; set; }
        public DayOfWeek DayOfWeek { get; set; } 
        public int TotalHours { get; set; } 

    }
}

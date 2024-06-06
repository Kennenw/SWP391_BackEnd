using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBad.BLL.DTO
{
    public class BookingDetailDTO
    {
        public int BookingDetailId { get; set; }
        public int? BookingId { get; set; }
        public int? SlotId { get; set; }
        public DateTime? Date { get; set; }
        public bool? Status { get; set; }
        public int? ScheludeId { get; set; }
        public int CourtNumberId { get; set; }
        public bool? CheckIn { get; set; }

    }
    public class SingleDayBookingRequest
    {
        public BookingDTO BookingS { get; set; }
        public BookingDetailDTO BookingDetailS { get; set; }
    }

    public class FlexibleScheduleRequest
    {
        public BookingDTO BookingF { get; set; }
        public ScheduleDTO ScheduleF { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class BookingDetailDTO
    {
        public int BookingDetailId { get; set; }

        public int? BookingId { get; set; }

        public int? SlotId { get; set; }

        public DateTime? Date { get; set; }

        public bool? Status { get; set; }

        public int? SubCourtId { get; set; }

        public double? TimeReducedInMinutes { get; set; }
    }
    
}

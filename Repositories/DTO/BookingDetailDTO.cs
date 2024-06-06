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

        public string? Date { get; set; }

        public string? Status { get; set; }

        public int? ScheludeId { get; set; }
    }
}

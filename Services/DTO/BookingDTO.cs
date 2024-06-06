using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBad.BLL.DTO
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
}

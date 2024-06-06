using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBad.BLL.DTO
{
    public class CourtNumberDTO
    {
        public int CourtNumberId { get; set; }
        public int? Number { get; set; }
        public int? CourtId { get; set; }
        public bool? Status { get; set; }
    }
}

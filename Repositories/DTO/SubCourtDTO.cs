using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class SubCourtDTO
    {
        public int SubCourtId { get; set; }
        public string? Number { get; set; }
        public bool? Status { get; set; }
        public int? CourtId { get; set; }

    }
    public class SubCourtGet
    {
        public int SubCourtId { get; set; }
        public string? Number { get; set; }
        public bool? Status { get; set; }
        public List<SlotTimeDTO> SlotTimes { get; set; }
    }

}

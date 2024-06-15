using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class AmenityCourtDTO
    {
        public int AmenityCourtId { get; set; }

        public int? AmenityId { get; set; }

        public int? CourtId { get; set; }
        public bool? Status { get; set; }
    }
}

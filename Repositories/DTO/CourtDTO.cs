using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DTO
{
    public class CourtDTO
    {
        public int CourtId { get; set; }

        public int? AreaId { get; set; }

        public string? CourtName { get; set; }

        public string? OpenTime { get; set; }

        public string? CloseTime { get; set; }

        public string? Rule { get; set; }

        public bool? Status { get; set; }
    }    
}

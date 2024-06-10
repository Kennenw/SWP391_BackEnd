using Microsoft.AspNetCore.Http;
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
        public int? ManagerId {  get; set; }
        public string? Rule { get; set; }

        public bool? Status { get; set; }
        public string Image { get; set; }
        public List<SubCourtDTO> SubCourts { get; set; }
        public List<AmenityCourtDTO> AmenityCourts { get; set; }
    }
    public class CourtCreateDTO
    {
        public int CourtId { get; set; }
        public int? AreaId { get; set; }
        public string? CourtName { get; set; }
        public string? OpenTime { get; set; }
        public string? CloseTime { get; set; }
        public int? ManagerId { get; set; }
        public string? Rule { get; set; }

        public bool? Status { get; set; }
        public IFormFile Image { get; set; }
        public List<SubCourtDTO> SubCourts { get; set; }
        public List<AmenityCourtDTO> AmenityCourts { get; set; }
    }
}

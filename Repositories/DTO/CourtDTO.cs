using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace Repositories.DTO
{
    public class CourtDTO
    {
        public int CourtId { get; set; }
        public int? AreaId { get; set; }
        public string? CourtName { get; set; }
        public string? OpenTime { get; set; }
        public string? CloseTime { get; set; }
        public string? Rules { get; set; }
        public bool? Status { get; set; }
        public string? Image { get; set; }
        public int? ManagerId { get; set; }
        public string? Title { get; set; }
        public string? Address { get; set; }
        public double? TotalRate { get; set; }
        public List<SubCourtDTO> SubCourts { get; set; }
        public List<AmenityDTO> Amenities { get; set; }
        public List<SlotTimeDTO> SlotTimes { get; set; }
    }
    public class Base64ImageModel
    {
        public string Base64Image { get; set; }
    }

    public class CourtDTOs
    {
        public int CourtId { get; set; }
        public int? AreaId { get; set; }
        public string? CourtName { get; set; }
        public string? OpenTime { get; set; }
        public string? CloseTime { get; set; }
        public string? Rules { get; set; }
        public bool? Status { get; set; }
        public string? Image { get; set; }
        public int? ManagerId { get; set; }
        public string? Title { get; set; }
        public string? Address { get; set; }
        public double? TotalRate { get; set; }
    }
}

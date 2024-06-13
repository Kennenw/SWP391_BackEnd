using Microsoft.AspNetCore.Http;
using Repositories;
using Repositories.DTO;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ICourtServices
    {
        public PagedResult<CourtDTO> GetCourts(int managerId, SortContent sortContent, int pageNumber, int pageSize);
        CourtDTO GetCourtById(int id);
        void UpdateCourt(int courtId,CourtDTO courtDTO);
        Task<Court> CreateCourtAsync(CourtDTO courtDTO);
        public PagedResult<CourtDTO> SearchCourts(string searchTerm, SortContent sortContent, int pageNumber, int pageSize);
        bool DeleteCourt(int id);
        Task UploadCourtImageAsync(int courtId, string base64Image);
    }
    public class CourtServices : ICourtServices
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly ImageServices _imageService;
        public CourtServices()
        {
            _unitOfWork ??= new UnitOfWork();
            _imageService = new ImageServices();
        }
        public PagedResult<CourtDTO> GetCourts(int managerId, SortContent sortContent,int pageNumber, int pageSize)
        {
            var courts = _unitOfWork.CourtRepo.GetAll();
            if (managerId != null)
            {
                courts = courts.Where(c => c.ManagerId == managerId).ToList();
            }
            switch (sortContent.sortCourtBy)
            {
                case SortCourtByEnum.CourtId:
                    courts = sortContent.sortType == SortTypeEnum.Ascending
                        ? courts.OrderBy(c => c.CourtId).ToList()
                        : courts.OrderByDescending(c => c.CourtId).ToList();
                    break;
                case SortCourtByEnum.CourtName:
                    courts = sortContent.sortType == SortTypeEnum.Ascending
                        ? courts.OrderBy(c => c.CourtName).ToList()
                        : courts.OrderByDescending(c => c.CourtName).ToList();
                    break;
                case SortCourtByEnum.ManagerId:
                    courts = sortContent.sortType == SortTypeEnum.Ascending
                        ? courts.OrderBy(c => c.ManagerId).ToList()
                        : courts.OrderByDescending(c => c.ManagerId).ToList();
                    break;
            }
            var totalItemCount = courts.Count;
            var pagedCourts = courts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var courtDTOs = pagedCourts.Select(c => new CourtDTO
            {
                    CourtId = c.CourtId,
                    AreaId = c.AreaId,
                    CourtName = c.CourtName,
                    OpenTime = c.OpenTime,
                    CloseTime = c.CloseTime,
                    ManagerId = c.ManagerId,
                    Image = c.Image,
                    Rule = c.Rules ,
                    Status = c.Status,
            }).ToList();
            
            return new PagedResult<CourtDTO>
            {
                Items = courtDTOs,
                TotalItem = totalItemCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public CourtDTO? GetCourtById(int id)
        {
            var court = _unitOfWork.CourtRepo.GetById(id);
            if (court == null)
            {
                return null;
            }
            var subCourts = _unitOfWork.SubCourtRepo.GetSubCourtByCourtId(court.CourtId);
            var amenityCourts = _unitOfWork.AmenityCourtRepo.GetAmenityByCourtId(court.CourtId);
            return new CourtDTO
            {
                CourtId = court.CourtId,
                AreaId = court.AreaId,
                CourtName = court.CourtName,
                OpenTime = court.OpenTime,
                CloseTime = court.CloseTime,
                ManagerId = court.ManagerId,
                Rule = court.Rules,
                Image = court.Image,
                Status = court.Status,
                SubCourts = subCourts.Select(sc => new SubCourtDTO
                {
                    SubCourtId = sc.SubCourtId,
                    CourtId = sc.CourtId,
                    Number = sc.Number,
                    Status = sc.Status
                }).ToList(),
                AmenityCourts = amenityCourts.Select(ac => new AmenityCourtDTO
                {
                    AmenityCourtId = ac.AmenityCourtId,
                    AmenityId = ac.AmenityId,
                    CourtId = ac.CourtId,
                }).ToList(),
            };
        }

        public void UpdateCourt(int id, CourtDTO courtDTO)
        {
            var court = _unitOfWork.CourtRepo.GetById(id);
            if (court != null)
            {
                court.AreaId = courtDTO.AreaId;
                court.CourtName = courtDTO.CourtName;
                court.OpenTime = courtDTO.OpenTime;
                court.CloseTime = courtDTO.CloseTime;
                court.Rules = courtDTO.Rule;
                court.Status = courtDTO.Status;
                _unitOfWork.CourtRepo.Update(court);
                _unitOfWork.SaveChanges();
            }
        }

        public async Task<Court> CreateCourtAsync(CourtDTO courtDTO)
        {
            var court = new Court
            {
                AreaId = courtDTO.AreaId,
                CourtName = courtDTO.CourtName,
                OpenTime = courtDTO.OpenTime,
                CloseTime = courtDTO.CloseTime,
                ManagerId = courtDTO.ManagerId,
                Rules = courtDTO.Rule,
                Status = true,
            };
            _unitOfWork.CourtRepo.Create(court);
            _unitOfWork.SaveChanges();
            foreach (var subCourt in courtDTO.SubCourts)
            {
                var subCourts = new SubCourt
                {
                    CourtId = court.CourtId,
                    Number = subCourt.Number,
                    Status = subCourt.Status
                };
                _unitOfWork.SubCourtRepo.Create(subCourts);
            }
            foreach (var amenityCourt in courtDTO.AmenityCourts)
            {
                var amenties = new AmenityCourt
                {
                    CourtId = court.CourtId,
                    AmenityId = amenityCourt.AmenityId,
                    Status = true
                };
                _unitOfWork.AmenityCourtRepo.Create(amenties);
            }
            foreach (var slot in courtDTO.SlotTime)
            {
                var slotTime = new SlotTime
                {
                    StartTime = slot.StartTime,
                    EndTime = slot.EndTime,
                    Price = slot.Price,
                    ManagerId = slot.ManagerId,
                    Status = true
                };
                _unitOfWork.SlotTimeRepo.Create(slotTime);
            }
            return court;
        }

        public bool DeleteCourt(int id)
        {
            var court = _unitOfWork.CourtRepo.GetById(id);
            if(court == null)
            {
                return false;
            }
            court.Status = false;
            _unitOfWork.CourtRepo.Update(court);
            _unitOfWork.SaveChanges();
            return true;
        }

        public PagedResult<CourtDTO> SearchCourts(string searchTerm, SortContent sortContent, int pageNumber, int pageSize)
        {
            var courts = _unitOfWork.CourtRepo.GetAll();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                courts = courts.Where(c =>
                    c.CourtName.Contains(searchTerm) ||
                    c.Area.Location.Contains(searchTerm)).ToList();
            }
            switch (sortContent.sortCourtBy)
            {
                case SortCourtByEnum.CourtId:
                    courts = sortContent.sortType == SortTypeEnum.Ascending
                        ? courts.OrderBy(c => c.CourtId).ToList()
                        : courts.OrderByDescending(c => c.CourtId).ToList();
                    break;
                case SortCourtByEnum.CourtName:
                    courts = sortContent.sortType == SortTypeEnum.Ascending
                        ? courts.OrderBy(c => c.CourtName).ToList()
                        : courts.OrderByDescending(c => c.CourtName).ToList();
                    break;
                case SortCourtByEnum.AreaId:
                    courts = sortContent.sortType == SortTypeEnum.Ascending
                        ? courts.OrderBy(c => c.AreaId).ToList()
                        : courts.OrderByDescending(c => c.AreaId).ToList();
                    break;
                case SortCourtByEnum.OpenTime:
                    courts = sortContent.sortType == SortTypeEnum.Ascending
                        ? courts.OrderBy(c => c.OpenTime).ToList()
                        : courts.OrderByDescending(c => c.OpenTime).ToList();
                    break;
            }
            var totalItemCount = courts.Count;
            var pagedCourts = courts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var courtDTOs = pagedCourts.Select(c => new CourtDTO
            {
                CourtId = c.CourtId,
                AreaId = c.AreaId,
                CourtName = c.CourtName,
                OpenTime = c.OpenTime,
                CloseTime = c.CloseTime,
                Rule = c.Rules,
                Image = c.Image,
                Status = c.Status
            }).ToList();

            return new PagedResult<CourtDTO>
            {
                Items = courtDTOs,
                TotalItem = totalItemCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task UploadCourtImageAsync(int courtId, string base64Image)
        {
            var court = _unitOfWork.CourtRepo.GetById(courtId);
            if (court == null) return;

            // Giải mã chuỗi base64 về mảng byte
            byte[] imageBytes = Convert.FromBase64String(base64Image);

            var fileName = Guid.NewGuid().ToString() + ".jpg";
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, fileName);

            await File.WriteAllBytesAsync(filePath, imageBytes);

            court.Image = fileName; // Lưu tên file vào cột Image

            _unitOfWork.CourtRepo.Update(court);
            _unitOfWork.SaveChanges();
        }

    }
}


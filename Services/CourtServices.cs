using Repositories;
using Repositories.DTO;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ICourtServices
    {
        public PagedResult<CourtDTO> GetCourts(SortContent sortContent, int pageNumber, int pageSize);
        CourtDTO GetCourtById(int id);
        void CreateCourt(CourtDTO court);
        void UpdateCourt(int id, CourtDTO court);
        void DeleteCourt(int id);
        public PagedResult<CourtDTO> SearchCourts(string searchTerm, SortContent sortContent, int pageNumber, int pageSize);

    }
    public class CourtServices : ICourtServices
    {
        private readonly UnitOfWork _unitOfWork;

        public CourtServices()
        {
            _unitOfWork ??= new UnitOfWork();
        }
        public PagedResult<CourtDTO> GetCourts(SortContent sortContent,int pageNumber, int pageSize)
        {
            var courts = _unitOfWork.CourtRepo.GetAll();
            var totalItemCount = courts.Count;
            var pagedCourts = courts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
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
            }
            var courtDTOs = pagedCourts.Select(c => new CourtDTO
            {
                CourtId = c.CourtId,
                    AreaId = c.AreaId,
                    CourtName = c.CourtName,
                    OpenTime = c.OpenTime,
                    CloseTime = c.CloseTime,
                    Rule = c.Rule,
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

        public CourtDTO GetCourtById(int id)
        {
            var court = _unitOfWork.CourtRepo.GetById(id);
            if (court == null)
            {
                return null;
            }
            return new CourtDTO
            {
                CourtId = court.CourtId,
                AreaId = court.AreaId,
                CourtName = court.CourtName,
                OpenTime = court.OpenTime,
                CloseTime = court.CloseTime,
                Rule = court.Rule,
                Status = court.Status,
            };
        }

        public void CreateCourt(CourtDTO courtDTO)
        {
            var court = new Court
            {
                AreaId = courtDTO.AreaId,
                CourtName = courtDTO.CourtName,
                OpenTime = courtDTO.OpenTime,
                CloseTime = courtDTO.CloseTime,
                Rule = courtDTO.Rule,
                Status = true,
            };
            _unitOfWork.CourtRepo.Create(court);
            _unitOfWork.SaveChanges();
        }

        public void UpdateCourt(int id, CourtDTO courtDTO)
        {
            var court = _unitOfWork.CourtRepo.GetById(id);
            court.AreaId = courtDTO.AreaId;
            court.CourtName = courtDTO.CourtName;
            court.OpenTime = courtDTO.OpenTime;
            court.CloseTime = courtDTO.CloseTime;
            court.Rule = courtDTO.Rule;
            court.Status = courtDTO.Status;
            _unitOfWork.CourtRepo.Update(court);
            _unitOfWork.SaveChanges();
            
        }

        public void DeleteCourt(int id)
        {
            var court = _unitOfWork.CourtRepo.GetById(id);
            court.Status = false;
            _unitOfWork.CourtRepo.Update(court);
            _unitOfWork.SaveChanges();
        }

        public PagedResult<CourtDTO> SearchCourts(string searchTerm, SortContent sortContent, int pageNumber, int pageSize)
        {
            var courts = _unitOfWork.CourtRepo.GetAll();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                courts = courts.Where(c =>
                    c.CourtName.Contains(searchTerm)).ToList();
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
                Rule = c.Rule,
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
    }
}


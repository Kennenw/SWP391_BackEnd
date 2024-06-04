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
    public interface IAmenityCourtServices
    {
        List<AmenityCourtDTO> GetAmenityCourts();
        AmenityCourtDTO GetAmenityCourtById(int id);
        public List<AmenityCourtDTO> GetAmenityByCourtId(int courtId);
        void CreateAmenityCourt(AmenityCourtDTO amenityCourt);
        void UpdateAmenityCourt(int id, AmenityCourtDTO amenityCourt);
        void DeleteAmenityCourt(int id);
    }
    public class AmenityCourtServices : IAmenityCourtServices
    {
        private readonly UnitOfWork _unitOfWork;

        public AmenityCourtServices()
        {
            _unitOfWork = new UnitOfWork();
        }

        public List<AmenityCourtDTO> GetAmenityCourts()
        {
            return _unitOfWork.AmenityCourtRepo.GetAll()
                .Select(ac => new AmenityCourtDTO
                {
                    AmenityCourtId = ac.AmenityCourtId,
                    CourtId = ac.CourtId,
                    AmenityId = ac.AmenityId
                }).ToList();
        }

        public AmenityCourtDTO GetAmenityCourtById(int id)
        {
            var ac = _unitOfWork.AmenityCourtRepo.GetById(id);
            if (ac == null)
            {
                return null;
            }
            return new AmenityCourtDTO
            {
                AmenityCourtId = ac.AmenityCourtId,
                CourtId = ac.CourtId,
                AmenityId = ac.AmenityId
            };
        }
        public void CreateAmenityCourt(AmenityCourtDTO amenityCourtDTO)
        {
            var ac = new AmenityCourt
            {
                CourtId = amenityCourtDTO.CourtId,
                AmenityId = amenityCourtDTO.AmenityId
            };
            _unitOfWork.AmenityCourtRepo.Create(ac);
            _unitOfWork.SaveChanges();
        }

        public void UpdateAmenityCourt(int id, AmenityCourtDTO amenityCourtDTO)
        {
            var ac = _unitOfWork.AmenityCourtRepo.GetById(id);
            if (ac != null)
            {
                ac.CourtId = amenityCourtDTO.CourtId;
                ac.AmenityId = amenityCourtDTO.AmenityId;
                ac.Status = true;
                _unitOfWork.AmenityCourtRepo.Update(ac);
                _unitOfWork.SaveChanges();
            }
        }

        public void DeleteAmenityCourt(int id)
        {
            var ac = _unitOfWork.AmenityCourtRepo.GetById(id);
            if (ac != null)
            {
                ac.Status = false;
                _unitOfWork.AmenityCourtRepo.Update(ac);
                _unitOfWork.SaveChanges();
            }
        }

        public List<AmenityCourtDTO> GetAmenityByCourtId(int courtId)
        {
            var amenityCourts = _unitOfWork.AmenityCourtRepo.GetAmenityByCourtId(courtId);
            return amenityCourts.Select(ac => new AmenityCourtDTO
            {
                AmenityCourtId = ac.AmenityCourtId,
                AmenityId = ac.AmenityId,
                CourtId = ac.CourtId
            }).ToList();
        }
    }
}

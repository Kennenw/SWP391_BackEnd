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
    public interface IAmenityServices
    {
        List<AmenityDTO> GetAmenities();
        AmenityDTO GetAmenityById(int id);
        void CreateAmenity(AmenityDTO amenity);
        void UpdateAmenity(int id, AmenityDTO amenity);
        void DeleteAmenity(int id);
    }

    public class AmenityServices : IAmenityServices
    {
        private readonly UnitOfWork _unitOfWork;

        public AmenityServices()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public List<AmenityDTO> GetAmenities()
        {
            return _unitOfWork.AmenityRepo.GetAll().
                Where(ar => ar.Status == true)
                .Select(amenity => new AmenityDTO
                {
                    AmenityId = amenity.AmenitiId,
                    Description = amenity.Description,
                    Status = amenity.Status,
                }).ToList();
        }

        public AmenityDTO GetAmenityById(int id)
        {
            var amenity = _unitOfWork.AmenityRepo.GetById(id);
            if (amenity == null || amenity.Status == false)
            {
                return null;
            }
            return new AmenityDTO
            {
                AmenityId = amenity.AmenitiId,
                Description = amenity.Description,
                Status = amenity.Status,
            };
        }

        public void CreateAmenity(AmenityDTO amenityDTO)
        {
            var amenity = new Amenity
            {
                Description = amenityDTO.Description,
                Status = true               
            };
            _unitOfWork.AmenityRepo.Create(amenity);
            _unitOfWork.SaveChanges();
        }

        public void UpdateAmenity(int id, AmenityDTO amenityDTO)
        {
            var amenity = _unitOfWork.AmenityRepo.GetById(id);
            if (amenity != null)
            {
                amenity.Description = amenityDTO.Description;
                amenity.Status = amenityDTO.Status;
                _unitOfWork.AmenityRepo.Update(amenity);
                _unitOfWork.SaveChanges();
            }
        }

        public void DeleteAmenity(int id)
        {
            var amenity = _unitOfWork.AmenityRepo.GetById(id);
            if (amenity != null)
            {
                amenity.Status = false;
                _unitOfWork.AmenityRepo.Update(amenity);
                _unitOfWork.SaveChanges();
            }
        }
    }
}

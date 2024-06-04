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
    public interface ICourtNumberServices
    {
        List<CourtNumberDTO> GetCourtNumbers();
        CourtNumberDTO GetCourtNumberById(int id);
        public List<CourtNumberDTO> GetCourtNumbersByCourtId(int courtId);
        void CreateCourtNumber(CourtNumberDTO courtNumber);
        void UpdateCourtNumber(int id, CourtNumberDTO courtNumber);
        void DeleteCourtNumber(int id);
    }
    public class CourtNumberServices : ICourtNumberServices
    {
        private readonly UnitOfWork _unitOfWork;

        public CourtNumberServices()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public List<CourtNumberDTO> GetCourtNumbers()
        {
            return _unitOfWork.CourtNumberRepo.GetAll()
                .Select(cn => new CourtNumberDTO
                {
                    CourtNumberId = cn.CourtNumberId,
                    Number = cn.Number,
                    CourtId = cn.CourtId
                }).ToList();
        }

        public List<CourtNumberDTO> GetCourtNumbersByCourtId(int courtId)
        {
            var courtNumbers = _unitOfWork.CourtNumberRepo.GetCourtNumber(courtId);
            return courtNumbers.Select(cn => new CourtNumberDTO
            {
                CourtNumberId = cn.CourtNumberId,
                Number = cn.Number,
                CourtId = cn.CourtId
            }).ToList();
        }

        public CourtNumberDTO GetCourtNumberById(int courtNumberId)
        {
            var courtNumber = _unitOfWork.CourtNumberRepo.GetById(courtNumberId);
            if (courtNumber == null)
            {
                return null;
            }

            return new CourtNumberDTO
            {
                CourtNumberId = courtNumber.CourtNumberId,
                Number = courtNumber.Number,
                CourtId = courtNumber.CourtId
            };
        }

        public void CreateCourtNumber(CourtNumberDTO courtNumberDTO)
        {
            var cn = new CourtNumber
            {
                Number = courtNumberDTO.Number,
                CourtId = courtNumberDTO.CourtId,
                Status = true
            };
            _unitOfWork.CourtNumberRepo.Create(cn);
            _unitOfWork.SaveChanges();
        }

        public void UpdateCourtNumber(int id, CourtNumberDTO courtNumberDTO)
        {
            var cn = _unitOfWork.CourtNumberRepo.GetById(id);
            if (cn != null)
            {
                cn.Number = courtNumberDTO.Number;
                cn.CourtId = courtNumberDTO.CourtId;
                cn.Status = true;
                _unitOfWork.CourtNumberRepo.Update(cn);
                _unitOfWork.SaveChanges();
            }
        }

        public void DeleteCourtNumber(int id)
        {
            var cn = _unitOfWork.CourtNumberRepo.GetById(id);
            if (cn != null)
            {
                cn.Status &= false;
                _unitOfWork.CourtNumberRepo.Update(cn);
                _unitOfWork.SaveChanges();
            }
        }
    }
}

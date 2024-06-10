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
    public interface ISubCourtServices
    {
        SubCourtDTO getSubCourt (int id);
        void createSubCourt(SubCourtDTO subCourtDTO);
        void updateSubCourt(int id, SubCourtDTO subCourtDTO);
        void deleteSubCourt(int id);
    }
    public class SubCourtServices : ISubCourtServices
    {
        private readonly UnitOfWork _unitOfWork;
        public SubCourtServices() 
        {
            _unitOfWork = new UnitOfWork();
        }
        public  void createSubCourt(SubCourtDTO subCourtDTO)
        {
            SubCourt subCourt = new SubCourt();
            subCourt.SubCourtId = subCourtDTO.SubCourtId;
            subCourt.Number = subCourtDTO.Number;
            subCourt.CourtId = subCourtDTO.CourtId;
            subCourt.Status = true; 
        }

        public void deleteSubCourt(int id)
        {
            var item = _unitOfWork.SubCourtRepo.GetById(id);
            if (item != null)
            {
                item.Status = false;
                _unitOfWork.SubCourtRepo.Update(item);
                _unitOfWork.SaveChanges();
            }
        }

        public SubCourtDTO getSubCourt(int id)
        {
            var item = _unitOfWork.SubCourtRepo.GetById(id);
            return new SubCourtDTO{ 
                SubCourtId = item.SubCourtId,
                Number = item.Number,
                CourtId = item.CourtId,
                Status = item.Status
            };
        }

        public void updateSubCourt(int id, SubCourtDTO subCourtDTO)
        {
            var item = _unitOfWork.SubCourtRepo.GetById(id);
            if(item != null)
            {
                item.Number = subCourtDTO.Number;
                item.Status = subCourtDTO.Status;
                _unitOfWork.SubCourtRepo.Update(item);
                _unitOfWork.SaveChanges();
            }
        }
    }
}

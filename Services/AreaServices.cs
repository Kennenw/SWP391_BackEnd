
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

    public interface IAreaServices
    {
        public List<AreaDTO> GetArea();
        public AreaDTO GetAreaById(int id);
        public void CreateArea(AreaDTO areaDTO);
        public void UpdateArea(int id, AreaDTO areaDTO);
        public void DeleteArea(int id);
    }
    public class AreaServices : IAreaServices
    {
        private readonly UnitOfWork _unitOfWork;

        public AreaServices()
        {
            _unitOfWork ??= new UnitOfWork();
        }
        public void CreateArea(AreaDTO areaDTO)
        {
            var area = new Area();
            area.Location = areaDTO.Location;
            area.Status = true;
            _unitOfWork.AreaRepo.Create(area);
            _unitOfWork.SaveChanges();
        }

        public void DeleteArea(int id)
        {
            var items = _unitOfWork.AreaRepo.GetById(id);
            if(items != null)
            {
                items.Status = false;
                _unitOfWork.AreaRepo.Update(items);
                _unitOfWork.SaveChanges();
            }
        }

        public List<AreaDTO> GetArea()
        {
            return _unitOfWork.AreaRepo.GetAll().
                Where(ar => ar.Status == true).
                Select(ar => new AreaDTO
            {
                AreaId = ar.AreaId,
                Location = ar.Location,
                Status = ar.Status,  
            }).ToList();
        }

        public AreaDTO GetAreaById(int id)
        {
            var item = _unitOfWork.AreaRepo.GetById(id);
            return new AreaDTO
            {
                AreaId = item.AreaId,
                Location = item.Location,
                Status = item.Status
            };
        }

        public void UpdateArea(int id, AreaDTO areaDTO)
        {
            var item = _unitOfWork.AreaRepo.GetById(id);
            item.Location = areaDTO.Location;
            item.Status = areaDTO.Status;
            _unitOfWork.AreaRepo.Update(item);
            _unitOfWork.SaveChanges();
        }
    }
}

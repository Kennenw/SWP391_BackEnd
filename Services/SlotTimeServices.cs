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
    public interface ISlotTimeServices
    {
        public List<SlotTimeDTO> GetSlot();
        public SlotTimeDTO GetSlotById(int id);
        public void CreateSlot(SlotTimeDTO slotTimeDTO);
        public void UpdateSlot(int id, SlotTimeDTO slotTimeDTO);
        public void DeleteSlot(int id);
    }

    public class SlotTimeServices : ISlotTimeServices
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly BookingServices _bookingServices;
        public SlotTimeServices()
        {
              _unitOfWork ??= new UnitOfWork();
              _bookingServices = new BookingServices();
        }
        public void CreateSlot(SlotTimeDTO slotTimeDTO)
        {
            SlotTime slot = new SlotTime
            {
                StartTime = slotTimeDTO.StartTime,
                EndTime = slotTimeDTO.EndTime,
                WeekendPrice = slotTimeDTO.WeekendPrice,
                WeekdayPrice = slotTimeDTO.WeekdayPrice,               
                Status = true,
            };
            _unitOfWork.SlotTimeRepo.Create(slot);
            _unitOfWork.SaveChanges();
        }

        public void DeleteSlot(int id)
        {
            var items = _unitOfWork.SlotTimeRepo.GetById(id);
            if(items != null)
            {
                items.Status = false;
                _unitOfWork.SlotTimeRepo.Update(items);
                _unitOfWork.SaveChanges();
            }
        }

        public List<SlotTimeDTO> GetSlot()
        {
            return _unitOfWork.SlotTimeRepo.GetAll().Select(slot => new SlotTimeDTO
            {
                SlotId = slot.SlotId,
                StartTime = slot.StartTime,
                EndTime = slot.EndTime,
                WeekdayPrice= slot.WeekendPrice,
                WeekendPrice = slot.WeekendPrice,   
                Status = slot.Status,
            }).ToList();
        }

        public SlotTimeDTO GetSlotById(int id)
        {
            var slot = _unitOfWork.SlotTimeRepo.GetById(id);
            if (slot != null)
            {
                return new SlotTimeDTO
                {
                    SlotId = slot.SlotId,
                    StartTime = slot.StartTime,
                    EndTime = slot.EndTime,
                    WeekdayPrice = slot.WeekdayPrice,
                    WeekendPrice = slot.WeekendPrice,
                    Status = slot.Status,
                };
            }return null;
        }

        public void UpdateSlot(int id, SlotTimeDTO slotTimeDTO)
        {
            var slot = _unitOfWork.SlotTimeRepo.GetById(id);
            if(slot != null)
            {
                slot.StartTime = slotTimeDTO.StartTime;
                slot.EndTime = slotTimeDTO.EndTime;
                slot.WeekendPrice = slotTimeDTO.WeekendPrice;
                slot.WeekdayPrice = slotTimeDTO.WeekdayPrice;
                slot.Status = slotTimeDTO.Status;
                _unitOfWork.SlotTimeRepo.Update(slot);
                _unitOfWork.SaveChanges();
            }

        }
    }
}

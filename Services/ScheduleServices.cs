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
    public interface IScheduleServices
    {
        public List<ScheduleDTO> GetSchedule();
        public List<ScheduleDTO> GetSlotByCourtN(int CourtNId);
        public ScheduleDTO GetScheduleById(int id);
        public void CreateSchedule(ScheduleDTO  scheduleDTO);
        public void UpdateSchedule(int id, ScheduleDTO scheduleDTO);
        public void DeleteSchedule(int id);
    }
    public class ScheduleServices : IScheduleServices
    {
        private readonly UnitOfWork _unitOfWork;

        public ScheduleServices()
        {
            _unitOfWork ??= new UnitOfWork();
        }
        public void CreateSchedule(ScheduleDTO scheduleDTO)
        {
            // Check for conflicts
            var existingSchedules = _unitOfWork.ScheduleRepo.GetAll();

            // Ensure no overlapping slot IDs for the same court number
            bool isCourtNumberConflict = existingSchedules.Any(s => s.SubCourtId == scheduleDTO.CourtNumberId && s.SlotId == scheduleDTO.SlotId);

            if (isCourtNumberConflict)
            {
                throw new InvalidOperationException("A schedule with the same court number and slot ID already exists.");
            }
            var schedule = new Schedule
            {
                ScheduleId = scheduleDTO.ScheduleId,
                SubCourtId = scheduleDTO.CourtNumberId,
                SlotId = scheduleDTO.SlotId,
                Status = true,
            };
            _unitOfWork.ScheduleRepo.Create(schedule);
            _unitOfWork.SaveChanges();
        }

        public void DeleteSchedule(int id)
        {
            var items = _unitOfWork.ScheduleRepo.GetById(id);
            if(items != null)
            {
                items.Status = false;
                _unitOfWork.ScheduleRepo.Update(items);
                _unitOfWork.SaveChanges();
            }
        }

        public List<ScheduleDTO> GetSlotByCourtN(int CourtId)
        {
            return _unitOfWork.ScheduleRepo.GetSlotByCourt(CourtId).Select(schedule => new ScheduleDTO
            {
                ScheduleId = schedule.ScheduleId,
                CourtNumberId = schedule.SubCourtId,
                SlotId = schedule.SlotId,
            }).ToList();
        }
        public List<ScheduleDTO> GetSchedule()
        {
            return _unitOfWork.ScheduleRepo.GetAll().Select(schedule => new ScheduleDTO 
            { 
                ScheduleId = schedule.ScheduleId,
                CourtNumberId = schedule.SubCourtId,
                SlotId = schedule.SlotId,
            }).ToList();
        }
        public ScheduleDTO GetScheduleById(int id)
        {
            var schedule = _unitOfWork.ScheduleRepo.GetById(id);
            return new ScheduleDTO
            {
                ScheduleId = schedule.ScheduleId,
                CourtNumberId = schedule.SubCourtId,
                SlotId = schedule.SlotId,
            };
        }

        public void UpdateSchedule(int id, ScheduleDTO scheduleDTO)
        {
            // Check for conflicts
            var existingSchedules = _unitOfWork.ScheduleRepo.GetAll();

            // Ensure no overlapping slot IDs for the same court number
            bool isCourtNumberConflict = existingSchedules.Any(s => s.SubCourtId == scheduleDTO.CourtNumberId && s.SlotId == scheduleDTO.SlotId);

            if (isCourtNumberConflict)
            {
                throw new InvalidOperationException("A schedule with the same court number and slot ID already exists.");
            }
            var schedule = _unitOfWork.ScheduleRepo.GetById(id);
            schedule.SubCourtId = scheduleDTO.CourtNumberId;
            schedule.SlotId = scheduleDTO.SlotId;
            _unitOfWork.ScheduleRepo.Update(schedule);
            _unitOfWork.SaveChanges();
        }
    }
}

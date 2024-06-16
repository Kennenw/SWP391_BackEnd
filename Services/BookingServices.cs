using Microsoft.AspNetCore.WebUtilities;
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
    public interface IBookingSevices
    {
        public List<BookingDTO> GetBooking();
        public List<BookingDTO> GetBookingByUserId(int id);
        public BookingDTO GetBookingById(int id);
        Task<BookedSlotDTO> BookFixedSchedule(FixedScheduleDTO scheduleDTO);
        Task<BookedSlotDTO> BookOneTimeSchedule(OneTimeScheduleDTO scheduleDTO);
        Task<BookedSlotDTO> BookFlexibleSchedule(FlexibleScheduleDTO scheduleDTO);
        Task<BookedSlotDTO> BookFlexibleSlot(BookedSlotDTO bookedSlotDTO);
        void CheckIn(int bookingDetailId);
        public void UpdateBooking(int id, BookingDTO bookingDTO);
        public void DeleteBooking(int id);
    }
    public class BookingServices : IBookingSevices
    {
        private readonly UnitOfWork _unitOfWork;


        public BookingServices()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public void DeleteBooking(int id)
        {
            var items = _unitOfWork.BookingRepo.GetById(id);
            if (items != null)
            {
                _unitOfWork.BookingRepo.Remove(items);
                _unitOfWork.SaveChanges();
            }
        }

        public List<BookingDTO> GetBooking()
        {
            return _unitOfWork.BookingRepo.GetAll().Select(booking => new BookingDTO
            {
                BookingId = booking.BookingId,
                CustomerId = booking.CustomerId,
                BookingTypeId = booking.BookingTypeId,
                PlayerQuantity = booking.PlayerQuantity,
                TotalPrice = booking.TotalPrice,
                Note = booking.Note,
            }).ToList();
        }

        public List<BookingDTO> GetBookingByUserId(int id)
        {
            return _unitOfWork.BookingRepo.GetBookingByUser(id).
                Select(booking => new BookingDTO
                {
                    BookingId = booking.BookingId,
                    CustomerId = booking.CustomerId,
                    BookingTypeId = booking.BookingTypeId,
                    PlayerQuantity = booking.PlayerQuantity,
                    TotalPrice = booking.TotalPrice,
                    Note = booking.Note,
                    Status = booking.Status,
                }).ToList();
        }

        public void UpdateBooking(int id, BookingDTO bookingDTO)
        {
            var booking = _unitOfWork.BookingRepo.GetById(id);
            if (booking != null)
            {
                booking.BookingId = bookingDTO.BookingId;
                booking.CustomerId = bookingDTO.CustomerId;
                booking.BookingTypeId = bookingDTO.BookingTypeId;
                booking.PlayerQuantity = bookingDTO.PlayerQuantity;
                booking.TotalPrice = bookingDTO.TotalPrice;
                booking.Note = bookingDTO.Note;
                _unitOfWork.BookingRepo.Update(booking);
                _unitOfWork.SaveChanges();
            }
        }
        public BookingDTO GetBookingById(int id)
        {
            var booking = _unitOfWork.BookingRepo.GetById(id);
            return new BookingDTO
            {
                BookingId = booking.BookingId,
                CustomerId = booking.CustomerId,
                BookingTypeId = booking.BookingTypeId,
                PlayerQuantity = booking.PlayerQuantity,
                TotalPrice = booking.TotalPrice,
                Note = booking.Note,
                Status = booking.Status,
            };
        }
        
        public void CheckIn(int bookingDetailId)
        {
            var bookingDetail = _unitOfWork.BookingDetailRepo.GetById(bookingDetailId);
            if (bookingDetail != null)
            {
                var checkIn = new CheckIn
                {
                    BookingDetail = bookingDetail,
                    CheckInTime = DateTime.Now,
                };
                _unitOfWork.CheckInRepo.Create(checkIn);
                _unitOfWork.SaveChanges();
            }
        }
        public async Task<BookedSlotDTO> BookFixedSchedule(FixedScheduleDTO scheduleDTO)
        {
            // Logic to book fixed schedule for >= 1 month
            // For simplicity, assuming 4 weeks in a month
            for (int i = 0; i < scheduleDTO.Months * 4; i++)
            {
                var bookedSlot = new BookedSlot
                {
                    CourtId = scheduleDTO.CourtId,
                    SubCourtId = scheduleDTO.SubCourtId,
                    UserId = scheduleDTO.UserId,
                    Date = DateTime.Now.AddDays(i * 7).Date, // Adjust date according to the day of the week
                    StartTime = scheduleDTO.StartTime,
                    EndTime = scheduleDTO.EndTime
                };
                _unitOfWork.BookedSlotRepo.Create(bookedSlot);
            }
            _unitOfWork.SaveChanges();
            return new BookedSlotDTO { /* Return booked slot details */ };
        }

        public async Task<BookedSlotDTO> BookOneTimeSchedule(OneTimeScheduleDTO scheduleDTO)
        {
            var bookedSlot = new BookedSlot
            {
                CourtId = scheduleDTO.CourtId,
                SubCourtId = scheduleDTO.SubCourtId,
                UserId = scheduleDTO.UserId,
                Date = scheduleDTO.Date,
                StartTime = scheduleDTO.StartTime,
                EndTime = scheduleDTO.EndTime
            };
            _unitOfWork.BookedSlotRepo.Create(bookedSlot);
            _unitOfWork.SaveChanges();
            return new BookedSlotDTO { /* Return booked slot details */ };
        }

        public async Task<BookedSlotDTO> BookFlexibleSchedule(FlexibleScheduleDTO scheduleDTO)
        {
            var flexibleSchedule = new FlexibleSchedule
            {
                CourtId = scheduleDTO.CourtId,
                SubCourtId = scheduleDTO.SubCourtId,
                UserId = scheduleDTO.UserId,
                TotalHours = scheduleDTO.TotalHours,
                RemainingHours = scheduleDTO.TotalHours
            };
            _unitOfWork.FlexibleScheduleRepo.Create(flexibleSchedule);
            _unitOfWork.SaveChanges();
            return new BookedSlotDTO { /* Return flexible schedule details */ };
        }

        public async Task<BookedSlotDTO> BookFlexibleSlot(BookedSlotDTO bookedSlotDTO)
        {
            var flexibleSchedule = _unitOfWork.FlexibleScheduleRepo.GetByUserId(bookedSlotDTO.UserId);
            if (flexibleSchedule == null || flexibleSchedule.RemainingHours <= 0)
            {
                throw new Exception("No remaining hours in flexible schedule");
            }

            var bookedSlot = new BookedSlot
            {
                CourtId = bookedSlotDTO.CourtId,
                SubCourtId = bookedSlotDTO.SubCourtId,
                UserId = bookedSlotDTO.UserId,
                Date = bookedSlotDTO.Date,
                StartTime = bookedSlotDTO.StartTime,
                EndTime = bookedSlotDTO.EndTime
            };
            _unitOfWork.BookedSlotRepo.Create(bookedSlot);
            flexibleSchedule.RemainingHours -= (bookedSlotDTO.EndTime - bookedSlotDTO.StartTime).TotalHours;
            _unitOfWork.SaveChanges();
            return new BookedSlotDTO { /* Return booked slot details */ };
        }
    }
}

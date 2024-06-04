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
        public void CreateBooking(BookingDTO bookingDTO);
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
        public void CreateBooking(BookingDTO bookingDTO)
        {
            var booking = new Booking
            {
                CustomerId = bookingDTO.CustomerId,
                BookingTypeId = bookingDTO.BookingTypeId,
                PlayerQuantity = bookingDTO.PlayerQuantity,
                TotalPrice = bookingDTO.TotalPrice,
                Note = bookingDTO.Note,
                Status = true
            };
            _unitOfWork.BookingRepo.Create(booking);
            _unitOfWork.SaveChanges();
        }

        public void DeleteBooking(int id)
        {
            var items = _unitOfWork.BookingRepo.GetById(id);
            if(items != null)
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
                PlayerQuantity= booking.PlayerQuantity,
                TotalPrice= booking.TotalPrice,
                Note = booking.Note,
            }).ToList();
        }

        public List<BookingDTO> GetBookingByUserId(int id)
        {
            return _unitOfWork.BookingRepo.GetBookingByUser(id).
                Select(booking => new BookingDTO
                {
                    BookingId= booking.BookingId,
                    CustomerId= booking.CustomerId,
                    BookingTypeId= booking.BookingTypeId,
                    PlayerQuantity= booking.PlayerQuantity,
                    TotalPrice= booking.TotalPrice,
                    Note = booking.Note,
                    Status = booking.Status,
                }).ToList();
        }

        public void UpdateBooking(int id, BookingDTO bookingDTO)
        {
            var booking = _unitOfWork.BookingRepo.GetById(id);
            if(booking != null)
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

        
        public void CreateSingleDayBooking(BookingDTO bookingDTO, BookingDetailDTO bookingDetailDTO)
        {
            // Validate and create booking
            var booking = new Booking
            {
                CustomerId = bookingDTO.CustomerId,
                BookingTypeId = bookingDTO.BookingTypeId,
                PlayerQuantity = bookingDTO.PlayerQuantity,
                TotalPrice = bookingDTO.TotalPrice,
                Note = bookingDTO.Note,
                Status = bookingDTO.Status
            };

            _unitOfWork.BookingRepo.Create(booking);
            _unitOfWork.SaveChanges();

            // Create booking detail
            var bookingDetail = new BookingDetail
            {
                BookingId = booking.BookingId,
                SlotId = bookingDetailDTO.SlotId,
                Date = bookingDetailDTO.Date,
                Status = bookingDetailDTO.Status
            };

            _unitOfWork.BookingDetailRepo.Create(bookingDetail);
            _unitOfWork.SaveChanges();
        }

        public void CreateFlexibleSchedule(BookingDTO bookingDTO, ScheduleDTO scheduleDTO)
        {
            // Validate and create booking
            var booking = new Booking
            {
                CustomerId = bookingDTO.CustomerId,
                BookingTypeId = bookingDTO.BookingTypeId,
                PlayerQuantity = bookingDTO.PlayerQuantity,
                TotalPrice = bookingDTO.TotalPrice,
                Note = bookingDTO.Note,
                Status = bookingDTO.Status
            };

            _unitOfWork.BookingRepo.Create(booking);
            _unitOfWork.SaveChanges();

            // Create schedule
            var schedule = new Schedule
            {
                CourtNumberId = scheduleDTO.CourtNumberId,
                SlotId = scheduleDTO.SlotId,
                BookingTypeId = scheduleDTO.BookingTypeId,
                StartDate = scheduleDTO.StartDate,
                EndDate = scheduleDTO.EndDate,
                TotalHours = scheduleDTO.TotalHours
            };

            _unitOfWork.ScheduleRepo.Create(schedule);
            _unitOfWork.SaveChanges();

            // Flexible schedule: player will book individual slots later
        }
    }
}

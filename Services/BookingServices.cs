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
        void CreateFixedBooking(FixedBookingDTO bookingDTO);
        void CreateSingleBooking(SingleBookingDTO bookingDTO);
        void CreateFlexibleBooking(FlexibleBookingDTO bookingDTO);
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

        public void CreateFixedBooking(FixedBookingDTO bookingDTO)
        {
            var startDate = DateTime.Now.Date;
            var endDate = startDate.AddMonths(bookingDTO.DurationMonths);
            var dayOfWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), bookingDTO.DayOfWeek, true);

            var booking = new Booking
            {
                CustomerId = bookingDTO.CustomerId,
                BookingTypeId = 1, 
                TotalPrice = bookingDTO.TotalPrice,
                Note = bookingDTO.Note,
                Status = true,
            };
            _unitOfWork.BookingRepo.Create(booking);
            _unitOfWork.SaveChanges();

            while (startDate <= endDate)
            {
                if (startDate.DayOfWeek == dayOfWeek)
                {
                    var bookingDetail = new BookingDetail
                    {
                        BookingId = booking.BookingId,
                        CourtNumberId = bookingDTO.CourtNumberId,
                        SlotId = GetSlotIdByTime(bookingDTO.StartTime),
                        Date = startDate,
                        Status = true,
                    };
                    _unitOfWork.BookingDetailRepo.Create(bookingDetail);
                }
                startDate = startDate.AddDays(1);
            }
            _unitOfWork.SaveChanges();
        }

        public void CreateSingleBooking(SingleBookingDTO bookingDTO)
        {
            var booking = new Booking
            {
                CustomerId = bookingDTO.CustomerId,
                BookingTypeId = 2, 
                TotalPrice = bookingDTO.TotalPrice,
                Note = bookingDTO.Note,
                Status = true,
            };
            _unitOfWork.BookingRepo.Create(booking);
            _unitOfWork.SaveChanges();

            var bookingDetail = new BookingDetail
            {
                BookingId = booking.BookingId,
                CourtNumberId = bookingDTO.CourtNumberId,
                SlotId = GetSlotIdByTime(bookingDTO.StartTime),
                Date = bookingDTO.Date,
                Status = true,
            };
            _unitOfWork.BookingDetailRepo.Create(bookingDetail);
            _unitOfWork.SaveChanges();
        }

        public void CreateFlexibleBooking(FlexibleBookingDTO bookingDTO)
        {
            var booking = new Booking
            {
                CustomerId = bookingDTO.CustomerId,
                BookingTypeId = 3, 
                TotalPrice = bookingDTO.TotalPrice,
                Note = bookingDTO.Note,
                TotalHours = bookingDTO.TotalHours,
                Status = true,
            };
            _unitOfWork.BookingRepo.Create(booking);
            _unitOfWork.SaveChanges();

        }

        public void CheckIn(int bookingDetailId)
        {
            var bookingDetail = _unitOfWork.BookingDetailRepo.GetById(bookingDetailId);
            if (bookingDetail != null)
            {
                bookingDetail.Status = true; 
                _unitOfWork.BookingDetailRepo.Update(bookingDetail);
                _unitOfWork.SaveChanges();
            }
        }
        private int GetSlotIdByTime(string time)
        {
            var slot = _unitOfWork.SlotTimeRepo.GetAll().FirstOrDefault(s => s.StartTime == time);
            return slot?.SlotId ?? 0;
        }
    }
}

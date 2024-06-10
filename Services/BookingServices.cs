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
        void CreateBooking(BookingRequestDTO bookingRequest);
        void CheckIn(int bookingDetailId);
        public void UpdateBooking(int id, BookingDTO bookingDTO);
        public void DeleteBooking(int id);
    }
    public class BookingServices : IBookingSevices
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IPaymentServices _vnPayService;

        public BookingServices()
        {
            _unitOfWork ??= new UnitOfWork();
        }
        public BookingServices(IPaymentServices vnPayService)
        {
            _unitOfWork ??= new UnitOfWork();
            _vnPayService = vnPayService;
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
        public void CreateBooking(BookingRequestDTO bookingRequest)
        {
            var bookingType = _unitOfWork.BookingTypeRepo.GetById(bookingRequest.BookingTypeId);
            var courtNumber = _unitOfWork.SubCourtRepo.GetById(bookingRequest.SubCourtId);
            var slotTime = _unitOfWork.SlotTimeRepo.GetById(bookingRequest.SlotId);
            if (bookingType == null || courtNumber == null || slotTime == null)
            {
                throw new Exception("Invalid booking details.");
            }
           
            var booking = new Booking
            {
                CustomerId = bookingRequest.CustomerId,
                BookingTypeId = bookingRequest.BookingTypeId,
                PlayerQuantity = bookingRequest.PlayerQuantity,
                TotalPrice = slotTime.Price.Value,
                Note = bookingRequest.Note,
                Status = true
            };
            _unitOfWork.BookingRepo.Create(booking);
            _unitOfWork.SaveChanges();

            switch (bookingRequest.BookingTypeId)
            {
                case 1: 
                    CreateFixedBooking(booking, bookingRequest);
                    break;

                case 2: 
                    CreateSingleBooking(booking, bookingRequest);
                    break;

                case 3: 
                    CreateFlexibleBooking(booking, bookingRequest);
                    break;

                default:
                    throw new Exception("Unknown booking type");
            }
        }

        public void CheckIn(int bookingDetailId)
        {
            var bookingDetail = _unitOfWork.BookingDetailRepo.GetById(bookingDetailId);
            if (bookingDetail != null)
            {
                bookingDetail.CheckIn = false;
                _unitOfWork.BookingDetailRepo.Update(bookingDetail);
                _unitOfWork.SaveChanges();
            }
        }

        private void CreateFixedBooking(Booking booking, BookingRequestDTO bookingRequest)
        {
            var startDate = DateTime.Now.Date;
            var endDate = startDate.AddMonths(bookingRequest.MonthsDuration); 

            while (startDate <= endDate)
            {
                if (startDate.DayOfWeek == bookingRequest.Date.DayOfWeek)
                {
                    var bookingDetail = new BookingDetail
                    {
                        BookingId = booking.BookingId,
                        SubCourtId = bookingRequest.SubCourtId,
                        SlotId = bookingRequest.SlotId,
                        Date = startDate,
                        Status = true,
                        CheckIn = true
                    };
                    _unitOfWork.BookingDetailRepo.Create(bookingDetail);
                }
                startDate = startDate.AddDays(1);
            }
            _unitOfWork.SaveChanges();
        }

        private void CreateSingleBooking(Booking booking, BookingRequestDTO bookingRequest)
        {
            var bookingDetail = new BookingDetail
            {
                BookingId = booking.BookingId,
                SubCourtId = bookingRequest.SubCourtId,
                SlotId = bookingRequest.SlotId,
                Date = bookingRequest.Date,
                Status = true,
                CheckIn = true,
            };
            _unitOfWork.BookingDetailRepo.Create(bookingDetail);
            _unitOfWork.SaveChanges();
        }

        private void CreateFlexibleBooking(Booking booking, BookingRequestDTO bookingRequest)
        {
            var totalHours = CalculateTotalHours(bookingRequest.SlotId); 
            booking.TotalHours = totalHours;
            booking.TotalPrice = (booking.TotalPrice / totalHours) * totalHours;
            _unitOfWork.BookingRepo.Update(booking);
        }

        private int CalculateTotalHours(int slotId)
        {
            var slotTime = _unitOfWork.SlotTimeRepo.GetById(slotId);
            if (slotTime == null)
            {
                throw new Exception("Invalid Slot");
            }

            var startTime = TimeSpan.Parse(slotTime.StartTime);
            var endTime = TimeSpan.Parse(slotTime.EndTime);
            var totalHours = (int)(endTime - startTime).TotalHours;

            return totalHours;
        }
    }
}

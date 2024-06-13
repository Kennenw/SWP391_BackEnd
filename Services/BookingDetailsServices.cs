using Repositories;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IBookingDetailsServices
    {
        public List<BookingDetail> GetBookingDetails();
        public BookingDetail GetBookingDetailsById(int id);     
        public void CreateBookingDetails(BookingDetail bookingDetail);
        public void UpdateBookingDetails(int id, BookingDetail bookingDetail);
        public void DeleteBookingDetails(int id);
    }
    public class BookingDetailsServices : IBookingDetailsServices
    {
        private readonly UnitOfWork _unitOfWork;

        public BookingDetailsServices()
        {
            _unitOfWork ??= new UnitOfWork();
        }
        public void CreateBookingDetails(BookingDetail bookingDetail)
        {
            _unitOfWork.BookingDetailRepo.Create(bookingDetail);
            _unitOfWork.SaveChanges();
        }

        public void DeleteBookingDetails(int id)
        {
            var items = _unitOfWork.BookingDetailRepo.GetById(id);
            if(items != null)
            {
                _unitOfWork.BookingDetailRepo.Remove(items);
                _unitOfWork.SaveChanges();
            }
        }

        public List<BookingDetail> GetBookingDetails()
        {
            return _unitOfWork.BookingDetailRepo.GetAll();
        }

        public BookingDetail GetBookingDetailsById(int id)
        {
            return _unitOfWork.BookingDetailRepo.GetById(id);
        }

        public void UpdateBookingDetails(int id, BookingDetail bookingDetail)
        {
            _unitOfWork.BookingDetailRepo.Update(id, bookingDetail);
            _unitOfWork.SaveChanges();
        }
        public void CheckIns(int bookingDetailId, int timeReducedInMinutes)
        {
            var bookingDetail = _unitOfWork.BookingDetailRepo.GetById(bookingDetailId);
            if (bookingDetail != null)
            {
                var checkIn = new CheckIn
                {
                    BookingDetailId = bookingDetailId,
                    CheckInTime = DateTime.Now,
                };

                _unitOfWork.CheckInRepo.Create(checkIn);
                _unitOfWork.SaveChanges();

                //bookingDetail.CheckInTime = DateTime.Now;
                //bookingDetail.TimeReducedInMinutes = timeReducedInMinutes;

                var booking = _unitOfWork.BookingRepo.GetById(bookingDetail.BookingId.Value);
                if (booking != null && booking.TotalHours.HasValue)
                {
                    var totalHoursReduced = timeReducedInMinutes / 60.0;
                    booking.TotalHours -= (int)totalHoursReduced;
                    _unitOfWork.BookingRepo.Update(booking);
                }

                _unitOfWork.BookingDetailRepo.Update(bookingDetail);
                _unitOfWork.SaveChanges();
            }
        }

    }
}

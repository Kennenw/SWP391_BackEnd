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
    }
}

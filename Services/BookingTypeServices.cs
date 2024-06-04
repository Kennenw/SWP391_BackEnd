using Repositories;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IBookingTypeServices
    {
        public List<BookingType> GetBookingType();
        public BookingType GetBookingTypeById(int id);   
        public void CreateBookingType(BookingType bookingType);
        public void UpdateBookingType(int id, BookingType bookingType);
        public void DeleteBookingType(int id);
    }
    public class BookingTypeServices : IBookingTypeServices
    {
        private readonly UnitOfWork _unitOfWork;

        public BookingTypeServices()
        {
            _unitOfWork ??= new UnitOfWork();
        }
        public void CreateBookingType(BookingType bookingType)
        {
            bookingType.Status = true;
            _unitOfWork.BookingTypeRepo.Create(bookingType);    
            _unitOfWork.SaveChanges();
        }

        public void DeleteBookingType(int id)
        {
            var items = _unitOfWork.BookingTypeRepo.GetById(id);
            if(items != null)
            {
                items.Status = false;
                _unitOfWork.BookingTypeRepo.Update(items);
                _unitOfWork.SaveChanges();
            }
        }

        public List<BookingType> GetBookingType()
        {
            return _unitOfWork.BookingTypeRepo.GetAll();
        }

        public BookingType GetBookingTypeById(int id)
        {
            return _unitOfWork.BookingTypeRepo.GetById(id);
        }

        public void UpdateBookingType(int id, BookingType bookingType)
        {
            _unitOfWork.BookingTypeRepo.Update(id, bookingType);
            _unitOfWork.SaveChanges();
        }
    }
}

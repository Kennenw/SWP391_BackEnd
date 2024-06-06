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
    public interface IBookingTypeServices
    {
        List<BookingTypeDTO> GetAllBookingTypes();
        BookingTypeDTO GetBookingTypeById(int id);
        void CreateBookingType(BookingTypeDTO bookingTypeDTO);
        void UpdateBookingType(int id, BookingTypeDTO bookingTypeDTO);
        void DeleteBookingType(int id);
    }
    public class BookingTypeServices : IBookingTypeServices
    {
        private readonly UnitOfWork _unitOfWork;

        public BookingTypeServices()
        {
            _unitOfWork ??= new UnitOfWork();
        }
        public List<BookingTypeDTO> GetAllBookingTypes()
        {
            var bookingTypes = _unitOfWork.BookingTypeRepo.GetAll();
            var bookingTypeDTOs = bookingTypes.Select(bt => new BookingTypeDTO
            {
                BookingTypeId = bt.BookingTypeId,
                Description = bt.Description,
                Status = (bool)bt.Status
            }).ToList();
            return bookingTypeDTOs;
        }

        public BookingTypeDTO GetBookingTypeById(int id)
        {
            var bookingType = _unitOfWork.BookingTypeRepo.GetById(id);
            if (bookingType == null)
            {
                return null;
            }

            return new BookingTypeDTO
            {
                BookingTypeId = bookingType.BookingTypeId,
                Description = bookingType.Description,
                Status = (bool)bookingType.Status
            };
        }

        public void CreateBookingType(BookingTypeDTO bookingTypeDTO)
        {
            var bookingType = new BookingType
            {
                Description = bookingTypeDTO.Description,
                Status = bookingTypeDTO.Status
            };
            _unitOfWork.BookingTypeRepo.Create(bookingType);
            _unitOfWork.SaveChanges();
        }

        public void UpdateBookingType(int id, BookingTypeDTO bookingTypeDTO)
        {
            var bookingType = _unitOfWork.BookingTypeRepo.GetById(id);
            if (bookingType != null)
            {
                bookingType.Description = bookingTypeDTO.Description;
                bookingType.Status = bookingTypeDTO.Status;
                _unitOfWork.BookingTypeRepo.Update(bookingType);
                _unitOfWork.SaveChanges();
            }
        }

        public void DeleteBookingType(int id)
        {
            var bookingType = _unitOfWork.BookingTypeRepo.GetById(id);
            if (bookingType != null)
            {
                bookingType.Status = false;
                _unitOfWork.BookingTypeRepo.Update(bookingType);
                _unitOfWork.SaveChanges();
            }
        }
    }
}

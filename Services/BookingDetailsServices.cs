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
    public interface IBookingDetailsServices
    {
        public List<BookingDetailDTO> GetBookingDetails();
        List<BookingDetailDTO> GetBookingDetailsByBookingId(int bookingId);
    }
    public class BookingDetailsServices : IBookingDetailsServices
    {
        private readonly UnitOfWork _unitOfWork;

        public BookingDetailsServices()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public List<BookingDetailDTO> GetBookingDetails()
        {
            return _unitOfWork.BookingDetailRepo.GetAll().
                Select(details => new BookingDetailDTO
                {
                    BookingDetailId = details.BookingDetailId,
                    BookingId = details.BookingId,
                    Date = details.Date,
                    SlotId = details.SlotId,
                    Status = details.Status,
                    SubCourtId = details.SubCourtId,
                    TimeReducedInMinutes = details.TimeReducedInMinutes,
                }).ToList();
        }

        public List<BookingDetailDTO> GetBookingDetailsByBookingId(int bookingId)
        {
            return _unitOfWork.BookingDetailRepo.GetAll()
                .Where(details => details.BookingId == bookingId)
                .Select(details => new BookingDetailDTO
                {
                    BookingDetailId = details.BookingDetailId,
                    BookingId = details.BookingId,
                    Date = details.Date,
                    SlotId = details.SlotId,
                    Status = details.Status,
                    SubCourtId = details.SubCourtId,
                    TimeReducedInMinutes = details.TimeReducedInMinutes,
                }).ToList();

        }
    }
}
using BookingBad.DAL;
using BookingBad.BLL.DTO;
using BookingBad.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBad.BLL.Services
{
    public interface IBookingSevices
    {
        public PagedResult<BookingDTO> GetBooking(SortContent sortContent, int pageNumber, int pageSize);
        public BookingDTO GetBookingById(int id);
        public void CreateBooking(BookingDTO bookingDTO);
        public void DeleteBooking(int id);
        public void CreateFixedSchedule(BookingDTO bookingDTO, ScheduleDTO scheduleDTO);
        public void CreateSingleDayBooking(BookingDTO bookingDTO, BookingDetailDTO bookingDetailDTO);
        public void CreateFlexibleSchedule(BookingDTO bookingDTO, ScheduleDTO scheduleDTO);
        public void CheckIn(int bookingDetailID);
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
                items.Status = false;
                _unitOfWork.BookingRepo.Update(items);
                _unitOfWork.SaveChanges();
            }
        }
        public PagedResult<BookingDTO> GetBooking(SortContent sortContent, int pageNumber, int pageSize)
        {
            var booking = _unitOfWork.BookingRepo.GetAll();
            switch (sortContent.sortBookingBy)
            {
                case SortBookingByEnum.BookingId:
                    booking = sortContent.sortType == SortTypeEnum.Ascending
                    ? booking.OrderBy(a => a.BookingId).ToList()
                    : booking.OrderByDescending(a => a.BookingId).ToList();
                    break;
                case SortBookingByEnum.CustomerId:
                    booking = sortContent.sortType == SortTypeEnum.Ascending
                    ? booking.OrderBy(a => a.CustomerId).ToList()
                    : booking.OrderByDescending(a => a.CustomerId).ToList();
                    break;
                case SortBookingByEnum.BookingTypeId:
                    booking = sortContent.sortType == SortTypeEnum.Ascending
                    ? booking.OrderBy(a => a.BookingTypeId).ToList()
                    : booking.OrderByDescending(a => a.BookingTypeId).ToList();
                    break;
                case SortBookingByEnum.TotalPrice:
                    booking = sortContent.sortType == SortTypeEnum.Ascending
                    ? booking.OrderBy(a => a.TotalPrice).ToList()
                    : booking.OrderByDescending(a => a.TotalPrice).ToList();
                    break;
            }
            var totalItemAccount = booking.Count;
            var pagedAccount = booking.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var bookingDTOs = pagedAccount.Select(b => new BookingDTO
            {
                BookingId = b.BookingId,
                CustomerId = b.CustomerId,
                BookingTypeId = b.BookingTypeId,
                PlayerQuantity = b.PlayerQuantity,
                TotalPrice = b.TotalPrice,
                Note = b.Note,
                Status = true
            }).ToList();
            return new PagedResult<BookingDTO>
            {
                Items = bookingDTOs,
                TotalItem = totalItemAccount,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
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
        public void CreateFixedSchedule(BookingDTO bookingDTO, ScheduleDTO scheduleDTO)
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

            // Create booking details for each week within the range
            for (DateTime date = scheduleDTO.StartDate.Value; date <= scheduleDTO.EndDate.Value; date = date.AddDays(7))
            {
                var bookingDetail = new BookingDetail
                {
                    BookingId = booking.BookingId,
                    SlotId = scheduleDTO.SlotId,
                    Date = date,
                    ScheludeId = schedule.ScheduleId,
                    CourtNumberId = scheduleDTO.CourtNumberId, // Set the CourtNumberID
                    Status = true,
                    CheckIn = false
                };

                _unitOfWork.BookingDetailRepo.Create(bookingDetail);
            }

            _unitOfWork.SaveChanges();
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
                ScheludeId = bookingDetailDTO.ScheludeId,
                CourtNumberId = bookingDetailDTO.CourtNumberId, // Set the CourtNumberID
                Status = bookingDetailDTO.Status,
                CheckIn = false
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

        public void CheckIn(int bookingDetailID)
        {
            var bookingDetail = _unitOfWork.BookingDetailRepo.GetById(bookingDetailID);
            if (bookingDetail != null)
            {
                bookingDetail.CheckIn = true;
                _unitOfWork.BookingDetailRepo.Update(bookingDetail);
                _unitOfWork.SaveChanges();
            }
        }
    }   
}

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
        List<BookingDTO> GetBookingsByCustomerId(int customerId);
        public BookingDTO GetBookingById(int id);
        Task<BookedSlotDTO> BookFixedSchedule(FixedScheduleDTO scheduleDTO);
        Task<BookedSlotDTO> BookOneTimeSchedule(OneTimeScheduleDTO scheduleDTO);
        Task<BookedSlotDTO> BookFlexibleSchedule(FlexibleScheduleDTO scheduleDTO);
        Task<BookedSlotDTO> BookFlexibleSlot(BookedSlotDTO bookedSlotDTO);
        Task<CheckInResponse> CheckIn(int subCourtId, int bookingDetailId);
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

        public List<BookingDTO> GetBookingsByCustomerId(int customerId)
        {
            return _unitOfWork.BookingRepo.GetAll()
                .Where(booking => booking.CustomerId == customerId)
                .Select(booking => new BookingDTO
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

        public async Task<BookedSlotDTO> BookFixedSchedule(FixedScheduleDTO scheduleDTO)
        {
            var slot = _unitOfWork.SlotTimeRepo.GetById(scheduleDTO.SlotTimeId);
            if (slot == null)
            {
                throw new Exception("Slot time not found");
            }
            var bookingDate = scheduleDTO.Date;
            bool isWeekend = (bookingDate.DayOfWeek == DayOfWeek.Saturday || bookingDate.DayOfWeek == DayOfWeek.Sunday);

            var booking = new Booking
            {
                CustomerId = scheduleDTO.UserId,
                CourtId = slot.CourtId,
                BookingTypeId = 1,
                Status = true,
                Note = scheduleDTO.Note,
                TotalPrice = isWeekend ? slot.WeekendPrice * 4 * scheduleDTO.Months : slot.WeekdayPrice * 4 * scheduleDTO.Months,
                StartDate = scheduleDTO.Date,
                EndDate = scheduleDTO.Date.AddMonths(scheduleDTO.Months)
            };
            _unitOfWork.BookingRepo.Create(booking);
            await _unitOfWork.SaveAsync();

            for (int i = 0; i < scheduleDTO.Months * 4; i++)
            {
                var bookedSlot = new BookingDetail
                {
                    BookingId = booking.BookingId,
                    SlotId = slot.SlotId,
                    SubCourtId = slot.SubCourtId,
                    Date = scheduleDTO.Date.AddDays(i * 7).Date,
                    Status = true
                };
                _unitOfWork.BookingDetailRepo.Create(bookedSlot);
                var checkIn = new CheckIn
                {
                    BookingDetailId = bookedSlot.BookingDetailId,
                };
                _unitOfWork.CheckInRepo.Create(checkIn);
                await _unitOfWork.SaveAsync();
            }
            return new BookedSlotDTO { BookingId = booking.BookingId, Date = DateTime.Now, SlotTimeId = slot.SlotId };
        }

        public async Task<BookedSlotDTO> BookOneTimeSchedule(OneTimeScheduleDTO scheduleDTO)
        {
            var slot = await _unitOfWork.SlotTimeRepo.GetByIdAsync(scheduleDTO.SlotTimeId);
            if (slot == null)
            {
                throw new Exception("Slot time not found");
            }
            var bookingDate = scheduleDTO.Date;
            bool isWeekend = (bookingDate.DayOfWeek == DayOfWeek.Saturday || bookingDate.DayOfWeek == DayOfWeek.Sunday);
            var booking = new Booking
            {
                CustomerId = scheduleDTO.UserId,
                CourtId = slot.CourtId,
                BookingTypeId = 2,
                Status = true,
                PlayerQuantity = scheduleDTO.PlayerQuantity,
                TotalPrice = isWeekend ? slot.WeekendPrice : slot.WeekdayPrice,
                StartDate = scheduleDTO.Date,
                EndDate = scheduleDTO.Date
            };
            _unitOfWork.BookingRepo.Create(booking);
            await _unitOfWork.SaveAsync();

            var bookedSlot = new BookingDetail
            {
                BookingId = booking.BookingId,
                SlotId = slot.SlotId,
                SubCourtId = slot.SubCourtId,
                Date = scheduleDTO.Date,
                Status = true
            };
            _unitOfWork.BookingDetailRepo.Create(bookedSlot);
            await _unitOfWork.SaveAsync();
            var checkIn = new CheckIn
            {
                BookingDetailId = bookedSlot.BookingDetailId,
            };
            _unitOfWork.CheckInRepo.Create(checkIn);
            await _unitOfWork.SaveAsync();
            return new BookedSlotDTO { BookingId = booking.BookingId, Date = scheduleDTO.Date, SlotTimeId = slot.SlotId };
        }

        public async Task<BookedSlotDTO> BookFlexibleSchedule(FlexibleScheduleDTO scheduleDTO)
        {
            var totalMinutes = scheduleDTO.TotalHours * 60;
            var court = _unitOfWork.CourtRepo.GetById(scheduleDTO.CourtId);
            var booking = new Booking
            {
                CustomerId = scheduleDTO.UserId,
                CourtId = scheduleDTO.CourtId,
                BookingTypeId = 3,
                Status = true,
                TotalHours = totalMinutes,
                TotalPrice = scheduleDTO.TotalHours * court.PricePerHour,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1)
            };
            _unitOfWork.BookingRepo.Create(booking);
            await _unitOfWork.SaveAsync();
            return new BookedSlotDTO { BookingId = booking.BookingId, Date = DateTime.Now, SlotTimeId = 0 };
        }

        public async Task<BookedSlotDTO> BookFlexibleSlot(BookedSlotDTO bookedSlotDTO)
        {
            var booking = await _unitOfWork.BookingRepo.GetByIdAsync(bookedSlotDTO.BookingId);
            if (booking == null || booking.TotalHours <= 0)
            {
                throw new Exception("No remaining hours in flexible schedule");
            }

            var slot = await _unitOfWork.SlotTimeRepo.GetByIdAsync(bookedSlotDTO.SlotTimeId);
            var courtId = slot.CourtId;
            var startTime = DateTime.Parse(slot.StartTime);
            var endTime = DateTime.Parse(slot.EndTime);
            if (slot == null)
            {
                throw new Exception("Slot time not found");
            }
            if (slot.CourtId != booking.CourtId)
            {
                throw new Exception("Slot time court ID does not match the booking court ID");
            }
            var bookedSlot = new BookingDetail
            {
                BookingId = bookedSlotDTO.BookingId,
                SlotId = slot.SlotId,
                SubCourtId = slot.SubCourtId,
                Date = bookedSlotDTO.Date,
                TimeReducedInMinutes = (endTime - startTime).TotalMinutes,
                Status = true
            };
            _unitOfWork.BookingDetailRepo.Create(bookedSlot);
            booking.TotalHours -= (endTime - startTime).TotalMinutes;
            if (booking.TotalHours < 0)
            {
                throw new Exception("Not enought time play");
            }
            else
            {
                _unitOfWork.BookingRepo.Update(booking);
                await _unitOfWork.SaveAsync();
            }
            var checkIn = new CheckIn
            {
                BookingDetailId = bookedSlot.BookingDetailId,
            };
            _unitOfWork.CheckInRepo.Create(checkIn);
            await _unitOfWork.SaveAsync();
            return new BookedSlotDTO { BookingId = bookedSlotDTO.BookingId, Date = bookedSlotDTO.Date, SlotTimeId = slot.SlotId };
        }


        public async Task<CheckInResponse> CheckIn(int subCourtId, int bookingDetailId)
        {
            var subCourt = _unitOfWork.SubCourtRepo.GetById(subCourtId);
            if (subCourt == null) return null;

            var bookingDetail = _unitOfWork.BookingDetailRepo.GetById(bookingDetailId);
            if (bookingDetail == null || bookingDetail.Status == false) return null;

            var slotId = _unitOfWork.BookingDetailRepo.GetSlotIdByBookingDetailId(bookingDetailId);
            var slotTime = _unitOfWork.SlotTimeRepo.GetById(slotId.Value);

            var courtId = _unitOfWork.SubCourtRepo.GetCourtIdBySubCourt(subCourtId);
            var court = _unitOfWork.CourtRepo.GetById(courtId.Value);

            bookingDetail.Status = false;
            _unitOfWork.BookingDetailRepo.Update(bookingDetail);

            var checkIn = _unitOfWork.CheckInRepo.GetAll()
                .FirstOrDefault(ci => ci.BookingDetailId == bookingDetailId);
            if (checkIn != null)
            {
                checkIn.CheckInTime = DateTime.Now;
                _unitOfWork.CheckInRepo.Update(checkIn);
            }

            await _unitOfWork.SaveAsync();
            return new CheckInResponse
            {
                CourtName = court.CourtName,
                SubCourtName = subCourt.Number,
                SlotTimeStart = slotTime.StartTime,
                SlotTimeEnd = slotTime?.EndTime
            };
        }

    }
}
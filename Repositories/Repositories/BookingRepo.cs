using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class BookingRepo : GenericRepository<Booking>
    {
        public BookingRepo() { }

        public int GetTotalBookingsCount()
        {
            return _dbSet.Count();
        }

        public int? GetCustomerIdByBookingId(int bookingId)
        {
            return _dbSet
                .Where(b => b.BookingId == bookingId)
                .Select(b => b.CustomerId)
                .FirstOrDefault();
        }
        public async Task<double> GetSuccessfulBookingRateAsync()
        {
            var totalBookings = await _dbSet.CountAsync();
            var successfulBookings = await _dbSet.CountAsync(b => b.Status == true);
            return (double)successfulBookings / totalBookings;
        }

        public void UpdateBookingStatus(int bookingId, bool status)
        {
            var booking = _dbSet.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking != null)
            {
                booking.Status = status;
                _context.SaveChanges();
            }
        }
    }
}

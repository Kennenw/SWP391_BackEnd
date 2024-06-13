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
        public List<Booking> GetBookingByUser(int id)
        {
            return _dbSet.Where(c => c.CustomerId == id).ToList();
        }
        public async Task<double> GetMonthlyRevenueAsync(int year, int month)
        {
            return await _dbSet.
                Where(b => b.StartDate.HasValue && b.StartDate.Value.Year == year && b.StartDate.Value.Month == month || b.Status == true).
                SumAsync(b => b.TotalPrice ?? 0);               
        }
    }
}

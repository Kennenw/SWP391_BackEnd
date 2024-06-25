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

        public async Task<double> GetRevenueAsync(int year, int month, int day)
        {
            return await _dbSet
                .Where(b => b.Status == true && 
                b.StartDate.HasValue && 
                b.StartDate.Value.Year == year && 
                b.StartDate.Value.Month == month && 
                b.StartDate.Value.Day == day)
                .SumAsync(b => b.TotalPrice ?? 0);
        }
    }
}

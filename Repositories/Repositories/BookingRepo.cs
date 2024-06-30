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

        public async Task<int> GetRevenueTotalBook(int? year, int? month, int? day)
        {
            var query = _dbSet.Where(b => b.Status == true && b.StartDate.HasValue);

            if (year.HasValue)
            {
                query = query.Where(b => b.StartDate.Value.Year == year.Value);
                if (month.HasValue && month.Value > 0)
                {
                    query = query.Where(b => b.StartDate.Value.Month == month.Value);
                    if (day.HasValue && day.Value > 0)
                    {
                        query = query.Where(b => b.StartDate.Value.Day == day.Value);
                    }
                }
            }

            return await query.CountAsync();
        }

    }
}

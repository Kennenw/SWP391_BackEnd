using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class PaymentRepo : GenericRepository<Payments>
    {
        public PaymentRepo() { }
        public async Task<double> GetRevenueAsync(int? year, int? month, int? day)
        {
            var query = _dbSet.Where(b => b.PaymentDate.HasValue);

            if (year.HasValue)
            {
                query = query.Where(b => b.PaymentDate.Value.Year == year.Value);

                if (month.HasValue && month.Value > 0)
                {
                    query = query.Where(b => b.PaymentDate.Value.Month == month.Value);

                    if (day.HasValue && day.Value > 0)
                    {
                        query = query.Where(b => b.PaymentDate.Value.Day == day.Value);
                    }
                }
            }

            return await query.SumAsync(b => b.TotalAmount ?? 0);
        }

    }
}

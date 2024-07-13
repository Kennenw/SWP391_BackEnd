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
        public async Task<List<object>> GetTotalRevenueByMonthYear()
        {
            var revenueByMonthYear = await _dbSet
                .Where(p => p.PaymentDate != null)
                .GroupBy(p => new { p.PaymentDate.Value.Year, p.PaymentDate.Value.Month, p.PaymentDate.Value.Day })
                .Select(g => new
                {
                    MonthYear = $"{g.Key.Year}-{g.Key.Month:D2}-{g.Key.Day}",
                    TotalAmount = g.Sum(p => p.TotalAmount)
                })
                .ToListAsync();

            return revenueByMonthYear.Cast<object>().ToList();
        }

        public Payments GetBookingId(int bookingId)
        {
            return _dbSet.FirstOrDefault(p => p.BookingId == bookingId );
        }
        public double GetTotalRevenue()
        {
            return _dbSet.Sum(b => b.TotalAmount ?? 0);
        }

        public Payments GetByPaymentCode(string paymentCode)
        {
            return _context.Payments.FirstOrDefault(p => p.PaymentCode == paymentCode);
        }
        public Payments  GetLatestPayment()
        {
            return  _dbSet.OrderByDescending(p => p.PaymentId).FirstOrDefault();
        }

    }
}

using BookingBad.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBad.DAL.Repositories
{
    public class BookingRepo : GenericRepository<Booking>
    {
        public BookingRepo() { }
        public List<Booking> GetBookingByUser(int id)
        {
            return _dbSet.Where(c => c.CustomerId == id).ToList();
        }
    }
}

using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class BookingDetailsRepo : GenericRepository<BookingDetail>
    {
        public BookingDetailsRepo() { }

        public int? GetSlotIdByBookingDetailId(int bookingDetailId)
        {
            var bookingDetail = _dbSet.FirstOrDefault(bd => bd.BookingDetailId == bookingDetailId);
            return bookingDetail?.SlotId;
        }

    }   
}

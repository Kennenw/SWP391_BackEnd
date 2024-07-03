using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class SlotTimeRepo : GenericRepository<SlotTime>
    {
        public SlotTimeRepo() { }

        public List<SlotTime> GetSlotTimeByCourtId(int courtId)
        {
            return _dbSet.Where(sc => sc.CourtId == courtId).ToList();
        }

        public List<SlotTime> GetSlotTimeByBookingId(int bookingId)
        {
            return _dbSet.Where(st => st.BookingDetails.Any(bd => bd.BookingId == bookingId)).ToList();
        }
    }
}

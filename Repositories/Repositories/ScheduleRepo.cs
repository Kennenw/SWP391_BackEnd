using BookingBad.DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBad.DAL.Repositories
{
    public class ScheduleRepo : GenericRepository<Schedule>
    {
        public ScheduleRepo() { }
        public List<Schedule> GetSlotByCourt(int id)
        {
            return _dbSet.Where(c => c.CourtNumberId == id).ToList();
        }

    }
}

using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
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

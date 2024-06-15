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

        public List<SlotTime> GeSlotTimeByCourtId(int courtId)
        {
            return _dbSet.Where(c => c.CourtId == courtId).ToList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookingBad.DAL.Entities;

namespace BookingBad.DAL.Repositories
{
    public class CourtNumberRepo : GenericRepository<CourtNumber>
    {
        public CourtNumberRepo() { }
        public IEnumerable<CourtNumber> GetCourtNumberId(int id)
        {
            return _dbSet.Where(c => c.CourtId == id).ToList();
        }

    }
}

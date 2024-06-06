using Microsoft.EntityFrameworkCore;
using BookingBad.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBad.DAL.Repositories
{
    public class AmenityCourtRepo : GenericRepository<AmenityCourt>
    {
        public AmenityCourtRepo() { }
        public List<AmenityCourt> GetAmenityByCourtId(int courtId)
        {
            return _dbSet.Where(c => c.CourtId == courtId)
                         .Include(c => c.Amenity)
                         .ToList();
        }
        public IEnumerable<AmenityCourt> GetByCourtId(int courtId)
        {
            return _dbSet.Where(ac => ac.CourtId == courtId).ToList();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Repositories.DTO;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
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
    }
}

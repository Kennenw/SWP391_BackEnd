using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repositories.Entities;

namespace Repositories.Repositories
{
    public class CourtNumberRepo : GenericRepository<CourtNumber>
    {
        public CourtNumberRepo() { }
        public List<CourtNumber> GetCourtNumber(int id)
        {
            return _dbSet.Where(c => c.CourtId == id).ToList();
        }
    }
}

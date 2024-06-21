using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class SubCourtRepo : GenericRepository<SubCourt>
    {
        public SubCourtRepo() { }
        public List<SubCourt> GetSubCourtByCourtId(int courtId)
        {
            return _dbSet.Where(sc => sc.CourtId == courtId).ToList();
        }

        public int? GetCourtIdBySubCourt(int subCourtId)
        {
            var court = _dbSet.FirstOrDefault(sc => sc.SubCourtId == subCourtId);
            return court?.CourtId;
        }
    }
}
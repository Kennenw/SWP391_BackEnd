using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class CourtRepo : GenericRepository<Court>
    {
        public CourtRepo() { }
        public int GetTotalCourtsCount()
        {
            return _dbSet.Where(ar => ar.Status == true).Count();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class PostRepo : GenericRepository<Post>
    {
        public PostRepo() { }
        public async Task<int> CountAsync()
        {
            return await _dbSet.Where(ar => ar.Status == true).CountAsync();
        }
    }
}

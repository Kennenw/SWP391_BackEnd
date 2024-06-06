
using BookingBad.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingBad.DAL.Repositories
{
    public class AccountRepo : GenericRepository<Account>
    {
        public AccountRepo()
        {
        }
        public Account GetAccountByEmailAndPassword(string email, string password)
        {
            return _dbSet.FirstOrDefault(a => a.Email == email && a.Password == password);
        }
    }
}

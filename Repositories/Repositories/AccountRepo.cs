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
    public enum UserRole
    {
        Admin = 1,
    }
    public class AccountRepo : GenericRepository<Account>
    {
        public AccountRepo()
        {
        }
        public Account GetAccountByEmailAndPassword(string email, string password)
        {
            return _dbSet.FirstOrDefault(a => a.Email == email && a.Password == password);
        }
        public Account GetAccountByEmail(string email )
        {
            return _dbSet.FirstOrDefault(a => a.Email == email );
        }
        public bool IsUserExist(string? email)
        {
            return _dbSet.FirstOrDefault(x => x.Email != email)!= null;
        }
        public bool IsAdmin(int user_id)
        {
            var res = false;
            var user = _dbSet.FirstOrDefault(x => x.AccountId == user_id);
            if (user != null && user.RoleId != null)
            {
                res = user.RoleId == (int)UserRole.Admin;
            }
            return res;
        }

        public int GetTotalAccountsCount()
        {
            return _dbSet.Where(ar => ar.Status == true).Count();
        }
    }
}

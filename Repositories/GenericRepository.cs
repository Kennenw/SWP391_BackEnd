using Microsoft.EntityFrameworkCore;
using Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class GenericRepository<T> where T : class
    {
        protected readonly BookingBadmintonSystemContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository()
        {
            _context ??= new BookingBadmintonSystemContext();
            _dbSet = _context.Set<T>();
        }

        public GenericRepository(BookingBadmintonSystemContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        #region Separating asign entity and save operators

        public void PrepareCreate(T entity)
        {
            _dbSet.Add(entity);
        }

        public void PrepareUpdate(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
        }

        public void PrepareRemove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        #endregion Separating asign entity and save operators

        public List<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public void Create(T entity)
        {
            if(entity != null)
            {
                _dbSet.Add(entity);
                _context.SaveChanges();
            }
        }
        
        public async Task<int> CreateAsync(T entity)
        {
            _dbSet.Add(entity);
            return await _context.SaveChangesAsync();
        }

        
        public bool Remove(T entity)
        {
           
             _dbSet.Remove(entity);
             _context.SaveChanges();
             return true;
            
        }

        public async Task<bool> RemoveAsync(T entity)
        {
            
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
            
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }


        public T GetById(string code)
        {
            return _dbSet.Find(code);
        }

        public T GetByName(string code)
        {
            return _dbSet.Find(code);
        } 
        public async Task<T> GetByIdAsync(string code)
        {
            return await _dbSet.FindAsync(code);
        }

        public T GetById(Guid code)
        {
            return _dbSet.Find(code);
        }

        public async Task<T> GetByIdAsync(Guid code)
        {
            return await _dbSet.FindAsync(code);
        }
        public void Update(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
            _context.SaveChanges();
        }

        public async Task<int> UpdateAsync(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }
        public void Update(int id,  T entity)
        {
            if(id != null)
            {
                var tracker = _context.Attach(entity);
                tracker.State = EntityState.Modified;
                _context.SaveChanges();
            }
        }
        public virtual int Count(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return query.Count();
        }
    }
}

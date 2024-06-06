using Microsoft.EntityFrameworkCore;
using BookingBad.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookingBad.DAL
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
        
        public bool Remove(T entity)
        {
           
             _dbSet.Remove(entity);
             _context.SaveChanges();
             return true;
            
        }
        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }
        public T GetByName(string code)
        {
            return _dbSet.Find(code);
        } 
 
        public void Update(T entity)
        {
            var tracker = _context.Attach(entity);
            tracker.State = EntityState.Modified;
            _context.SaveChanges();
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
    }
}

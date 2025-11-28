using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;

namespace WebApi.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ImsDbContext _context;

        public GenericRepository(ImsDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        
        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<List<T>> GetWhereAsync(
            Expression<Func<T, bool>> predicate,
            bool tracked = false,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>().Where(predicate);
        
            if (!tracked)
                query = query.AsNoTracking();
        
            if (includes != null)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }
        
            return await query.ToListAsync();
        }

    }
}
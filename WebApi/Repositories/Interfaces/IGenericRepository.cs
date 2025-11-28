using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApi.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task<T> GetByIdAsync(int id);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<List<T>> GetWhereAsync(
            Expression<Func<T, bool>> predicate,
            bool tracked = false,
            params Expression<Func<T, object>>[] includes);
    }

}
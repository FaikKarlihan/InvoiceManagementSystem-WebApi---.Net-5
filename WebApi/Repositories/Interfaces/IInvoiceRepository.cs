using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApi.Entities;

namespace WebApi.Repositories
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        Task<Invoice> GetWithAllDetailsByIdAsync(int id);
        Task RemoveRangeAsync(IReadOnlyCollection<Invoice> invoices);
        Task DeleteWhereAsync(Expression<Func<Invoice, bool>> predicate);
        Task AddRangeAsync(IEnumerable<Invoice> invoices);
        Task<List<Invoice>> GetAllWithHousings();
    }
}
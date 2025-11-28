using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Entities;

namespace WebApi.Repositories
{
    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(ImsDbContext context) : base(context){}

        public async Task<Invoice> GetWithAllDetailsByIdAsync(int id)
        {
            return await _context.Invoices
                    .Include(i => i.Housing)
                        .ThenInclude(u => u.User)
                    .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task DeleteWhereAsync(Expression<Func<Invoice, bool>> predicate)
        {
            var invoices = _context.Set<Invoice>().Where(predicate);
            _context.Set<Invoice>().RemoveRange(invoices);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveRangeAsync(IReadOnlyCollection<Invoice> invoices)
        {
            _context.Invoices.RemoveRange(invoices);
            await _context.SaveChangesAsync();
        }
        public async Task AddRangeAsync(IEnumerable<Invoice> invoices)
        {
            await _context.Invoices.AddRangeAsync(invoices);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Invoice>> GetAllWithHousings()
        {
            return await _context.Invoices
                .Include(h=> h.Housing)
                .OrderBy(e => EF.Property<int>(e, "Id"))
                .ToListAsync();
        }    
    }
}
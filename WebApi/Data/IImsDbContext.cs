using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities;

namespace WebApi.Data
{
    public interface IImsDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Dues> Dues { get; set; }
        DbSet<Housing> Housings { get; set; }
        DbSet<Invoice> Invoices { get; set; }
        DbSet<Message> Messages { get; set; }
        DbSet<Payment> Payments { get; set; }
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}


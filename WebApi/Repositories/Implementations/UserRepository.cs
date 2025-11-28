using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Entities;

namespace WebApi.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ImsDbContext context) : base(context){}

        public async Task<User> GetWithAllDetailsByMailAsync(string mail)
        {
            return await _context.Users
                    .Include(u => u.Housing)
                        .ThenInclude(h => h.Invoices)
                    .AsNoTracking().FirstOrDefaultAsync(u => u.Mail == mail);
        }
        public async Task<bool> UserExistsAsync(string mail)
        {
            return await _context.Users.AsNoTracking().AnyAsync(u => u.Mail == mail);
        }
        public async Task<User> GetByMailAsync(string mail, bool tracked = false)
        {
            IQueryable<User> query = _context.Users.Include(u => u.Housing);
        
            if (!tracked)
                query = query.AsNoTracking();
        
            return await query.FirstOrDefaultAsync(u => u.Mail == mail);
        }
        public async Task AddMessageAsync(Message message)
        {
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
        }
        public async Task SavePasswordAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task<List<User>> GetAllWithHousings()
        {
            return await _context.Users
                .Include(h => h.Housing)
                .OrderBy(e => EF.Property<int>(e, "Id"))
                .ToListAsync();
        }
        public async Task AddPaymentAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Payment>> GetUserPaymentsAsync(string mail)
        {
            return await _context.Payments
                    .Where(x => x.UserMail == mail)
                    .OrderBy(e => EF.Property<int>(e, "Id"))
                    .AsNoTracking().ToListAsync();
        }
        public async Task<List<Payment>> GetAllPaymentsAsync()
        {
            return await _context.Payments
                    .OrderByDescending(e => EF.Property<int>(e, "Id"))
                    .AsNoTracking().ToListAsync();
        }
        public async Task<List<Message>> GetAllMessagesAsync()
        {
            return await _context.Messages
                    .Include(u=> u.Sender)
                    .OrderByDescending(e => EF.Property<int>(e, "Id"))
                    .AsNoTracking().ToListAsync();
        }
    }
}
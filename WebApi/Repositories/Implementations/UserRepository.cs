using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Entities;

namespace WebApi.Repositories
{
    //  Inherits basic CRUD operations from GenericRepository<User>
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ImsDbContext context) : base(context){}

        public async Task<User> GetWithAllDetailsByMailAsync(string mail)
        {
            return await _context.Users
                    .Include(u => u.Housing)
                        .ThenInclude(h => h.Invoices)
                    .FirstOrDefaultAsync(u => u.Mail == mail);
        }

        public async Task<bool> UserExistsAsync(string mail)
        {
            return await _context.Users.AnyAsync(u => u.Mail == mail);
        }
        public async Task<User> GetByMailAsync(string mail)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Mail == mail);
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
    }
}
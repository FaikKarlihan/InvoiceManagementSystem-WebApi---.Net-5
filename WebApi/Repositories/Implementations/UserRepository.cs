using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Entities;

namespace WebApi.Repositories
{
    //  Inherits basic CRUD operations from GenericRepository<User>
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ImsDbContext _context;
        public UserRepository(ImsDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetWithAllDetailsByIdAsync(int id)
        {
            return await _context.Users
                    .Include(u => u.Housing)
                        .ThenInclude(h => h.Invoices)
                    .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> UserExistsAsync(string mail)
        {
            return await _context.Users.AnyAsync(u => u.Mail == mail);
        }
        public async Task<User> GetByMailAsync(string mail)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Mail == mail);
        }
    }
}
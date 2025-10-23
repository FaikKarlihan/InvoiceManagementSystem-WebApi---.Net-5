using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Entities;

namespace WebApi.Repositories
{
    public class HousingRepository : GenericRepository<Housing>, IHousingRepository
    {
        public HousingRepository(ImsDbContext context) : base(context){}

        public async Task<Housing> GetWithAllDetailsByApartmentNumberAsync(string apartmentNumber)
        {
            return await _context.Housings
                    .Include(h => h.User)
                    .Include(i => i.Invoices)
                    .FirstOrDefaultAsync(h => h.ApartmentNumber == apartmentNumber);
        }

        public async Task<bool> HousingExistsAsync(string apartmentNumber)
        {
            return await _context.Housings.AnyAsync(h => h.ApartmentNumber == apartmentNumber);
        }
        public async Task<Housing> GetByApartmentNumberAsync(string apartmentNumber)
        {
            return await _context.Housings.FirstOrDefaultAsync(h => h.ApartmentNumber == apartmentNumber);
        }

        public async Task<bool> IsOccupiedAsync(string apartmentNumber)
        {
            return await _context.Housings.AnyAsync(h => h.ApartmentNumber == apartmentNumber && h.UserId != null);
        }
        public async Task AssignToHousingAsync(User user, Housing housing, bool isOwner)
        {
            housing.UserId = user.Id;
            user.HousingId = housing.Id;
            housing.IsOwner = isOwner;
            await _context.SaveChangesAsync();
        }
        public async Task UnassignFromHousingAsync(Housing housing)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == housing.UserId);
            if (user != null)
                user.HousingId = null;
                
            housing.UserId = null;
            housing.IsOwner = false;
            await _context.SaveChangesAsync();
        }
        public async Task<List<int>> GetAllIdsAsync()
        {
            return await _context.Housings
                .Select(h => h.Id)
                .ToListAsync();        
        }
    }
}
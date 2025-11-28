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
                    .AsNoTracking().FirstOrDefaultAsync(h => h.ApartmentNumber == apartmentNumber);
        }
        public async Task<bool> HousingExistsAsync(string apartmentNumber)
        {
            return await _context.Housings
                .AsNoTracking()
                .AnyAsync(h => h.ApartmentNumber == apartmentNumber);
        }
        public async Task<Housing> GetByApartmentNumberAsync(string apartmentNumber, bool tracked = false)
        {
            IQueryable<Housing> query = _context.Housings.Include(h => h.User);
        
            if (!tracked)
                query = query.AsNoTracking();
        
            return await query.FirstOrDefaultAsync(h => h.ApartmentNumber == apartmentNumber);
        }
        public async Task<List<Housing>> GetAllHousingsAsync()
        {
            return await _context.Housings
                .Include(h => h.User)
                    .AsNoTracking()
                    .ToListAsync();
        }
        public async Task<bool> IsOccupiedAsync(string apartmentNumber)
        {
            return await _context.Housings
                .AsNoTracking()
                .AnyAsync(h => h.ApartmentNumber == apartmentNumber && h.UserId != null);
        }
        public async Task AssignToHousingAsync(User user, Housing housing, bool isOwner)
        {
            // Housing tablosundaki UserId alanını set ediyoruz
            // Bu foreign key, konut ile kullanıcı arasındaki ilişkiyi belirler
            housing.UserId = user.Id;  

            // Konut sahibinin mi kiracı mı olduğunu belirten flag
            housing.IsOwner = isOwner;

            // Navigation property'leri güncelliyoruz (opsiyonel ama iyi olur)
            // Böylece bellek içindeki nesneler senkron kalır
            housing.User = user;  // Housing üzerinden User'a erişim
            user.Housing = housing; // User üzerinden Housing'a erişim

            await _context.SaveChangesAsync();
        }
        public async Task UnassignFromHousingAsync(Housing housing)
        {
            // Konuta bağlı kullanıcıyı buluyoruz
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == housing.UserId);

            // Eğer kullanıcı bulunursa, kullanıcının konut referansını null yapıyoruz
            // Bu, birebir ilişkide navigation property ile senkron kalmasını sağlar
            if (user != null)
                user.HousingId = null; // User tablosunda FK varsa onu kaldırıyoruz (opsiyonel ama iyi)

            // Konut tablosundaki ilişkiyi ve IsOwner bilgisini sıfırlıyoruz
            housing.UserId = null;  // FK null yapılır
            housing.IsOwner = null;  // Sahiplik bilgisi kaldırılır

            // Navigation property'leri de null yapıyoruz
            // Bu, bellek içindeki nesnelerin senkron kalması için iyi bir uygulamadır
            housing.User = null;

            await _context.SaveChangesAsync();
        }
        public async Task<List<int>> GetAllHousingIdsWithUsersAsync()
        {
            return await _context.Housings
                .Where(h => h.UserId != null)
                .AsNoTracking()
                .Select(h => h.Id)
                .ToListAsync();
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Entities;

namespace WebApi.Repositories
{
    public interface IHousingRepository : IGenericRepository<Housing>
    {
        Task<Housing> GetWithAllDetailsByApartmentNumberAsync(string apartmentNumber);
        Task<List<int>> GetAllIdsAsync();
        Task<bool> HousingExistsAsync(string apartmentNumber);
        Task<Housing> GetByApartmentNumberAsync(string apartmentNumber);
        Task<bool> IsOccupiedAsync(string apartmentNumber);
        Task AssignToHousingAsync(User user, Housing housing, bool isOwner);
        Task UnassignFromHousingAsync(Housing housing);
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Dtos;
using WebApi.Entities;

namespace WebApi.Services
{
    public interface IHousingService
    {
        // Commands
        Task HousingCreateAsync(HousingCreateDto dto);
        Task HousingUpdateAsync(HousingUpdateDto dto, string apartmentNumber);
        Task HousingDeleteAsync(string apartmentNumber);
        Task UnassignFromHousingAsync(string apartmentNumber);


        // Queries
        Task<HousingDetailViewModel> GetHousingDetailAsync(string apartmentNumber);
        Task<List<HousingDetailViewModel>> GetAllHousingsAsync();
        Task<HousingWithInvoicesViewModel> GetHousingWithInvoicesAsync(string apartmentNumber);
    }
}
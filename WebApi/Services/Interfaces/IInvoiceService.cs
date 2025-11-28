using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Common;
using WebApi.Dtos;

namespace WebApi.Services
{
    public interface IInvoiceService
    {
        // Commands
        Task InvoiceCreateAsync(InvoiceCreateDto dto);
        Task AssignDuesToAllAsync(DuesCreateDto dto);
        Task InvoiceDeleteAsync(int id);
        Task InvoiceUpdateAsync(InvoiceUpdateDto dto, int id);

        // Queries
        Task<InvoiceDetailViewModel> GetInvoiceDetailAsync(int id);
        Task<InvoiceWithAllDetailsViewModel> GetWithAllDetailsAsync(int id);
        Task<List<InvoiceDetailViewModel>> GetInvoicesByTypeAsync(InvoiceType? type = null, bool all = false);
    }
}
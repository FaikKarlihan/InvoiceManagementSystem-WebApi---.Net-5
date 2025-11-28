using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WebApi.Common;
using WebApi.Dtos;
using WebApi.Entities;
using WebApi.Repositories;

namespace WebApi.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IMapper _mapper;
        private readonly IInvoiceRepository _invoiceRepo;
        private readonly IHousingRepository _housingRepo;
        public InvoiceService(IMapper mapper, IInvoiceRepository repository, IHousingRepository housingRepository)
        {
            _mapper = mapper;
            _invoiceRepo = repository;
            _housingRepo = housingRepository;
        }

        // Commands
        public async Task InvoiceCreateAsync(InvoiceCreateDto dto)
        {
            var invoice = _mapper.Map<Invoice>(dto);
            var housing = await _housingRepo.GetWithAllDetailsByApartmentNumberAsync(dto.ApartmentNumber);

            if (housing is null)
                throw new InvalidOperationException("No housing found for this apartment number.");
            if (housing.User is null)
                throw new InvalidOperationException("There is no resident in the housing! Invoice cannot be assigned.");

            if (!Enum.TryParse(dto.Type.Trim(), true, out InvoiceType type))
                throw new InvalidOperationException("Invoice Type not found. Please enter 'Bill' or 'Due'.");

            // The old dues for the same month are overridden. -- Aynı aya ait eski aidatlar geçersiz kılınır.
            var existingDues = await _invoiceRepo.GetWhereAsync(i =>
                i.HousingId == housing.Id &&
                i.Month == dto.Month &&
                i.Year == dto.Year &&
                i.Type == InvoiceType.Dues);

            var exDues = existingDues.FirstOrDefault();
            if (exDues != null)
            {
                // same id
                exDues.Amount = dto.Amount;
                exDues.DueDate = dto.DueDate;
                exDues.PaymentInfo = dto.PaymentInfo+" -Updated dues.";
                await _invoiceRepo.UpdateAsync(exDues);
                return;
            }

            invoice.Type = type;
            invoice.HousingId = housing.Id;
            await _invoiceRepo.AddAsync(invoice);
        }
        public async Task InvoiceDeleteAsync(int id)
        {
            if (id < 0)
                throw new InvalidOperationException("Id cannot be less than 0.");

            var invoice = await _invoiceRepo.GetByIdAsync(id);
            if (invoice is null)
                throw new InvalidOperationException("Invoice not found.");
            await _invoiceRepo.DeleteAsync(invoice);
        }
        public async Task InvoiceUpdateAsync(InvoiceUpdateDto dto, int id)
        {
            if (id < 0)
                throw new InvalidOperationException("Id cannot be less than 0.");

            var invoice = await _invoiceRepo.GetByIdAsync(id) ?? throw new InvalidOperationException("Invoice not found.");

            InvoiceType? type = null;
            if (!string.IsNullOrWhiteSpace(dto.Type))
            {
                if (!Enum.TryParse(dto.Type.Trim(), true, out InvoiceType parsedType))
                    throw new InvalidOperationException("Invalid invoice type.");
                type = parsedType;
            }

#nullable enable
            Housing? housing = null;
            if (!string.IsNullOrWhiteSpace(dto.ApartmentNumber))
            {
                housing = await _housingRepo.GetWithAllDetailsByApartmentNumberAsync(dto.ApartmentNumber)
                    ?? throw new InvalidOperationException("No housing found for this apartment number.");
            }
#nullable disable

            if (type == InvoiceType.Dues && housing != null)
            {
                // Delete old dues from the same month
                await _invoiceRepo.DeleteWhereAsync(i =>
                    i.HousingId == housing.Id &&
                    i.Month == dto.Month &&
                    i.Year == dto.Year &&
                    i.Type == InvoiceType.Dues);
            }

            _mapper.Map(dto, invoice);

            if (type.HasValue)
                invoice.Type = type.Value;

            if (housing != null)
                invoice.HousingId = housing.Id;

            await _invoiceRepo.UpdateAsync(invoice);
        }
        public async Task AssignDuesToAllAsync(DuesCreateDto dto)
        {
            var housingIds = await _housingRepo.GetAllHousingIdsWithUsersAsync();
            if (!housingIds.Any())
                throw new InvalidOperationException("There are no residents in any housing!");

            // Current dues for the relevant month. -- İlgili aya ait güncel aidatlar.
            var existingDues = await _invoiceRepo.GetWhereAsync(i =>
                i.Type == InvoiceType.Dues &&
                i.Month == dto.Month &&
                i.Year == dto.Year);

            // Get the IDs of the residences that already have dues. -- Aidatı olan konutların id'leri alınır.
            var excludedIds = existingDues.Select(i => i.HousingId).ToHashSet();

            var invoices = new List<Invoice>();

            foreach (var id in housingIds)
            {
                // If this property already has dues, skip it. -- Eğer konutun aidatı varsa, atlanır.
                if (excludedIds.Contains(id))
                    continue;

                var invoice = _mapper.Map<Invoice>(dto);
                invoice.HousingId = id;
                invoice.Type = InvoiceType.Dues;
                invoice.PaymentInfo = $"Mass dues appointment for {dto.Month}/{dto.Year}.";
                invoice.DueDate = DateTime.UtcNow.AddMonths(1);

                invoices.Add(invoice);
            }

            if (invoices.Count == 0)
                throw new InvalidOperationException("All housings already have dues for this month.");

            await _invoiceRepo.AddRangeAsync(invoices);
        }

        // Queries
        public async Task<InvoiceDetailViewModel> GetInvoiceDetailAsync(int id)
        {
            if (id < 0)
                throw new InvalidOperationException("Id cannot be less than 0.");

            var invoice = await _invoiceRepo.GetWithAllDetailsByIdAsync(id);
            if (invoice is null)
                throw new InvalidOperationException("Invoice not found.");

            return _mapper.Map<InvoiceDetailViewModel>(invoice);
        }
        public async Task<InvoiceWithAllDetailsViewModel> GetWithAllDetailsAsync(int id)
        {
            if (id < 0)
                throw new InvalidOperationException("Id cannot be less than 0.");

            var invoice = await _invoiceRepo.GetWithAllDetailsByIdAsync(id);
            if (invoice is null)
                throw new InvalidOperationException("Invoice not found.");

            return _mapper.Map<InvoiceWithAllDetailsViewModel>(invoice); 
        }
        public async Task<List<InvoiceDetailViewModel>> GetInvoicesByTypeAsync(InvoiceType? type = null, bool all = false)
        {
            if (!all && !type.HasValue)
                throw new InvalidOperationException("You must specify a type or set 'all' to true.");

            var invoices = all
                ? await _invoiceRepo.GetAllWithHousings()
                : await _invoiceRepo.GetWhereAsync(i => i.Type == type.Value, includes: h => h.Housing);

            return _mapper.Map<List<InvoiceDetailViewModel>>(invoices);
        }
    }
}
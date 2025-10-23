using System;
using System.Collections.Generic;
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
            var housing = await _housingRepo.GetWithAllDetailsByIdAsync(dto.ApartmentNumber);

            if (!Enum.TryParse(dto.Type.Trim(), true, out InvoiceType type))
                throw new InvalidOperationException("Invoice Type not found. Please enter 'Bill' or 'Due'.");
            if (housing is null)
                throw new InvalidOperationException("No housing found for this apartment number.");

            if (type == InvoiceType.Dues)
            {
                // Delete old dues from the same month
                await _invoiceRepo.DeleteWhereAsync(i =>
                    i.HousingId == housing.Id &&
                    i.Month == dto.Month &&
                    i.Year == dto.Year &&
                    i.Type == InvoiceType.Dues);
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
                housing = await _housingRepo.GetWithAllDetailsByIdAsync(dto.ApartmentNumber)
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
            var housingIds = await _housingRepo.GetAllIdsAsync();
            var invoices = new List<Invoice>();

            foreach (var id in housingIds)
            {
                var invoice = _mapper.Map<Invoice>(dto);
                invoice.HousingId = id;
                invoice.Type = InvoiceType.Dues;
                invoice.PaymentInfo = $"Mass dues appointment for {dto.Month}/{dto.Year}.";
                invoice.DueDate = DateTime.UtcNow.AddMonths(1);

                invoices.Add(invoice);
            }

            await _invoiceRepo.AddRangeAsync(invoices);
        }

        // Queries
        public async Task<InvoiceDetailViewModel> GetInvoiceDetailAsync(int id)
        {
            if (id < 0)
                throw new InvalidOperationException("Id cannot be less than 0.");

            var invoice = await _invoiceRepo.GetByIdAsync(id);
            if (invoice is null)
                throw new InvalidOperationException("Invoice not found.");

            return _mapper.Map<InvoiceDetailViewModel>(invoice);
        }
        public async Task<List<InvoiceDetailViewModel>> GetInvoicesByTypeAsync(InvoiceType? type = null, bool all = false)
        {
            if (!all && !type.HasValue)
                throw new InvalidOperationException("You must specify a type or set 'all' to true.");

            var invoices = all
                ? await _invoiceRepo.GetAllAsync()
                : await _invoiceRepo.GetWhereAsync(i => i.Type == type.Value);

            return _mapper.Map<List<InvoiceDetailViewModel>>(invoices);
        }
    }
}
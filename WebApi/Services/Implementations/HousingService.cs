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
    public class HousingService : IHousingService
    {
        private readonly IHousingRepository _housingRepo;
        private readonly IInvoiceRepository _invoiceRepo;
        private readonly IMapper _mapper;
        public HousingService(IHousingRepository housingRepository, IMapper mapper, IInvoiceRepository invoiceRepository)
        {
            _housingRepo = housingRepository;
            _invoiceRepo = invoiceRepository;
            _mapper = mapper;
        }
        
        // Commands
        public async Task HousingCreateAsync(HousingCreateDto dto)
        {
            if (await _housingRepo.HousingExistsAsync(dto.ApartmentNumber))
                throw new InvalidOperationException("Housing already exists in the apartment number.");

            var housing = _mapper.Map<Housing>(dto);

            if (!Enum.TryParse(dto.PlanType, true, out PlanType planType))
                throw new InvalidOperationException("Invalid PlanType value. Valid values (OnePlusOne, TwoPlusOne, ThreePlusOne)");
            housing.PlanType = planType;
            await _housingRepo.AddAsync(housing);
        }
        public async Task HousingUpdateAsync(HousingUpdateDto dto, string apartmentNumber)
        {
            if (string.IsNullOrWhiteSpace(apartmentNumber))
                throw new InvalidOperationException("apartmentNumber cannot be empty.");

            var housing = await _housingRepo.GetByApartmentNumberAsync(apartmentNumber.Trim(), true);
            if (housing is null)
                throw new InvalidOperationException("No housing found with this apartmentNumber.");

            if (!string.IsNullOrWhiteSpace(dto.ApartmentNumber) && dto.ApartmentNumber != apartmentNumber)
            {
                if (await _housingRepo.HousingExistsAsync(dto.ApartmentNumber.Trim()))
                    throw new InvalidOperationException("Housing already exists in this ApartmentNumber.");
            }
            
            PlanType? planType = null;
            if (!string.IsNullOrWhiteSpace(dto.PlanType))
            {
                if (!Enum.TryParse(dto.PlanType.Trim(), true, out PlanType ParsedType))
                    throw new InvalidOperationException("Invalid PlanType value.");
                planType = ParsedType;
            }

            if (planType.HasValue)
                housing.PlanType = planType.Value;

            _mapper.Map(dto, housing);
            await _housingRepo.UpdateAsync(housing);
        }
        public async Task HousingDeleteAsync(string apartmentNumber)
        {
            if (string.IsNullOrWhiteSpace(apartmentNumber))
                throw new InvalidOperationException("apartmentNumber cannot be empty.");

            var housing = await _housingRepo.GetByApartmentNumberAsync(apartmentNumber.Trim(), true);
            if (housing is null)
                throw new InvalidOperationException("No housing found with this apartmentNumber.");

            if (housing.UserId != null)
                throw new InvalidOperationException("Housing cannot be deleted! It has a resident.");

            var invoices = await _invoiceRepo.GetWhereAsync(i => i.HousingId == housing.Id, true);
            if (invoices.Any())
                await _invoiceRepo.RemoveRangeAsync(invoices);

            await _housingRepo.DeleteAsync(housing);
        }
        public async Task UnassignFromHousingAsync(string apartmentNumber)
        {
            if (string.IsNullOrWhiteSpace(apartmentNumber))
                throw new InvalidOperationException("apartmentNumber cannot be empty.");

            var housing = await _housingRepo.GetByApartmentNumberAsync(apartmentNumber.Trim(), true);
            if (housing is null)
                throw new InvalidOperationException("No housing found with this apartmentNumber.");
            
            await _housingRepo.UnassignFromHousingAsync(housing);
        }


        // Queries 
        public async Task<HousingDetailViewModel> GetHousingDetailAsync(string apartmentNumber)
        {
            if (string.IsNullOrWhiteSpace(apartmentNumber))
                throw new InvalidOperationException("apartmentNumber cannot be empty.");

            var housing = await _housingRepo.GetByApartmentNumberAsync(apartmentNumber.Trim());
            if (housing is null)
                throw new InvalidOperationException("No housing found with this apartmentNumber.");

            return _mapper.Map<HousingDetailViewModel>(housing);
        }
        public async Task<List<HousingDetailViewModel>> GetAllHousingsAsync()
        {
            var housings = await _housingRepo.GetAllHousingsAsync();
            return _mapper.Map<List<HousingDetailViewModel>>(housings);
        }
        public async Task<HousingWithInvoicesViewModel> GetHousingWithInvoicesAsync(string apartmentNumber)
        {
            if (string.IsNullOrWhiteSpace(apartmentNumber))
                throw new InvalidOperationException("apartmentNumber cannot be empty.");

            var housing = await _housingRepo.GetWithAllDetailsByApartmentNumberAsync(apartmentNumber);
            if (housing is null)
                throw new InvalidOperationException("No housing found with this apartmentNumber."); 
            
            return _mapper.Map<HousingWithInvoicesViewModel>(housing);
        }
    }
}
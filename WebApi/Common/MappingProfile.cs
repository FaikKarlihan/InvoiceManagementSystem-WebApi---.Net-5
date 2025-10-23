using AutoMapper;
using WebApi.Dtos;
using WebApi.Entities;

namespace WebApi.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // CREATE USER
            CreateMap<UserCreateDto, User>()
                .ForMember(dest => dest.Role, opt => opt.Ignore());

            // SEND MESSAGE
            CreateMap<UserMessageDto, Message>();

            // UPDATE USER
            CreateMap<UserUpdateDto, User>()
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) =>
                        srcMember != null &&
                        (srcMember is not string str || !string.IsNullOrWhiteSpace(str))));

            // USER DETAIL
            CreateMap<User, UserDetailViewModel>()
                .ForMember(dest => dest.NameSurname, opt => opt.MapFrom(src => src.Name + " " + src.Surname))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

            // HOUSING CREATE
            CreateMap<HousingCreateDto, Housing>()
                .ForMember(dest => dest.ApartmentStatus, opt => opt.Ignore())
                .ForMember(dest => dest.PlanType, opt => opt.Ignore());
            
            // HOUSING UPDATE
            CreateMap<HousingUpdateDto, Housing>()
                .ForMember(dest => dest.PlanType, opt => opt.Ignore())
                .ForMember(dest => dest.ApartmentStatus, opt => opt.Ignore())
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) =>
                        srcMember != null &&
                        (srcMember is not string str || !string.IsNullOrWhiteSpace(str))));

            // HOUSING DETAIL
            CreateMap<Housing, HousingDetailViewModel>()
                .ForMember(dest => dest.ApartmentStatus, opt => opt.MapFrom(src => src.ApartmentStatus.ToString()))
                .ForMember(dest => dest.PlanType, opt => opt.MapFrom(src => src.PlanType.ToString()))
                .ForMember(dest => dest.OwnershipStatus, opt => opt.MapFrom(src => src.IsOwner ? "Owner" : "Tenant"))
                .ForMember(dest => dest.Resident, opt => opt.
                        MapFrom(src => src.User != null ? src.User.Name + " " + src.User.Surname : "No resident yet."));
                 

            // INVOICE DETAIL
            CreateMap<Invoice, InvoiceDetailViewModel>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.Period, opt => opt.MapFrom(src => src.Month + "/" + src.Year));

            // INVOICE CREATE
            CreateMap<InvoiceCreateDto, Invoice>()
                .ForMember(dest => dest.HousingId, opt => opt.Ignore())
                .ForMember(dest => dest.Type, opt => opt.Ignore());

            // INVOICE UPDATE
            CreateMap<InvoiceUpdateDto, Invoice>()
                .ForMember(dest => dest.Type, opt => opt.Ignore())
                .ForMember(dest => dest.HousingId, opt => opt.Ignore())
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) =>
                        srcMember != null &&
                        (srcMember is not string str || !string.IsNullOrWhiteSpace(str))));

            // DUES TO ALL
            CreateMap<DuesCreateDto, Invoice>();    

            // USER WITH HOUSING
            CreateMap<User, UserWithHousingViewModel>();

            // USER WITH INVOICES
            CreateMap<User, UserWithInvoiceViewModel>();

            // USER WITH ALL DETAIL
            CreateMap<User, UserWithAllDetailsViewModel>();
        }
    }
}
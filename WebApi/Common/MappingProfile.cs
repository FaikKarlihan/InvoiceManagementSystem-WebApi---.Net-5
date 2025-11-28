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
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
                .ForMember(dest => dest.ApartmentNumber,
                    opt => opt.MapFrom(src => src.Housing != null ? src.Housing.ApartmentNumber : "No housing yet"));

            // MESSAGES
            CreateMap<Message, MessagesDto>()
                .ForMember(dest=> dest.PostedBy, opt=> opt.MapFrom(src=> src.Sender.Name+" "+src.Sender.Surname));
    
            // HOUSING CREATE
            CreateMap<HousingCreateDto, Housing>()
                .ForMember(dest => dest.PlanType, opt => opt.Ignore());

            // HOUSING UPDATE
            CreateMap<HousingUpdateDto, Housing>()
                .ForMember(dest => dest.PlanType, opt => opt.Ignore())
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) =>
                        srcMember != null &&
                        (srcMember is not string str || !string.IsNullOrWhiteSpace(str))));

            // HOUSING DETAIL
            CreateMap<Housing, HousingDetailViewModel>()
                .ForMember(dest => dest.ApartmentStatus, opt => opt.MapFrom(src => src.ApartmentStatus.ToString()))
                .ForMember(dest => dest.PlanType, opt => opt.MapFrom(src => src.PlanType.ToString()))
                .ForMember(dest => dest.OwnershipStatus,
                    opt => opt.MapFrom(src =>
                        src.IsOwner == null ? "No resident yet"
                        : src.IsOwner == true ? "Owner."
                        : "Tenant"))
                .ForMember(dest => dest.Resident, opt => opt.
                        MapFrom(src => src.User != null ? src.User.Name + " " + src.User.Surname : "No resident yet"));


            // INVOICE DETAIL
            CreateMap<Invoice, InvoiceDetailViewModel>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus.ToString()))   
                .ForMember(dest => dest.OverdueStatus, opt => opt.MapFrom(
                                src => src.PaymentStatus == PaymentStatus.Paid ? "None" : src.OverdueStatus.ToString())) 
                .ForMember(dest => dest.Period, opt => opt.MapFrom(src => src.Month + "/" + src.Year))
                .ForMember(dest => dest.ApartmentNumber, opt => opt.MapFrom(src => src.Housing.ApartmentNumber));

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
            CreateMap<User, UserWithHousingViewModel>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Housing, opt => opt.MapFrom(src => src.Housing));

            // USER WITH INVOICES
            CreateMap<User, UserWithInvoiceViewModel>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Invoices, opt => opt.MapFrom(src => src.Housing.Invoices));

            // USER WITH ALL DETAILS
            CreateMap<User, UserWithAllDetailsViewModel>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Housing, opt => opt.MapFrom(src => src.Housing))
                .ForMember(dest => dest.Invoices, opt => opt.MapFrom(src => src.Housing.Invoices));

            // HOUSING WITH INVOICES
            CreateMap<Housing, HousingWithInvoicesViewModel>()
                .ForMember(dest => dest.Housings, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Invoices, opt => opt.MapFrom(src => src.Invoices));

            // INVOICE WITH ALL DETAILS
            CreateMap<Invoice, InvoiceWithAllDetailsViewModel>()
                .ForMember(dest => dest.Invoices, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.Housing.User))
                .ForMember(dest => dest.Housing, opt => opt.MapFrom(src => src.Housing));
                
        }
    }
}
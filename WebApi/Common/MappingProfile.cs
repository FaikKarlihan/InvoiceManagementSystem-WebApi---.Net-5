using AutoMapper;
using WebApi.Dtos;
using WebApi.Entities;

namespace WebApi.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserCreateDto, User>().ForMember(dest => dest.Role, opt => opt.Ignore());
        }
    }
}
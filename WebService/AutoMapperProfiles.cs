using AutoMapper;
using Common.Messages.DTOS;
using Common.Models;

namespace AuthenticationAPI;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AspNetRole, RoleDTO>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        CreateMap<Application, ApplicationDTO>();
        CreateMap<User, UserDTO>();
        CreateMap<User, RoleDTO>();
        CreateMap<UserApplication, UserApplicationDTO>();
        CreateMap<AspNetUserRoles, UserRoleDTO>();
    }
}
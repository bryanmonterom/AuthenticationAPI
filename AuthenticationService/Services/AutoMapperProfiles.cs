using AutoMapper;
using Common.Messages.DTOS;
using Common.Models;

namespace AuthenticationService.Services;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AspNetRole, RoleDTO>();
    }
}
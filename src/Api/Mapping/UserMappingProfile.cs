using AutoMapper;
using Contracts.v1.Users;
using Domain.Users;

namespace Api.Mapping;

public sealed class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserResponse>()
            .ForCtorParam("FullName", o => o.MapFrom(s => s.FullName));
    }
}
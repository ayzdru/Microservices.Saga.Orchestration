using AutoMapper;
using Identity.Api.Dtos.PersistedGrants;
using Identity.BusinessLogic.Dtos.Grant;

namespace Identity.Api.Mappers;

public class PersistedGrantApiMapperProfile : Profile
{
    public PersistedGrantApiMapperProfile()
    {
        CreateMap<PersistedGrantDto, PersistedGrantApiDto>(MemberList.Destination);
        CreateMap<PersistedGrantDto, PersistedGrantSubjectApiDto>(MemberList.Destination);
        CreateMap<PersistedGrantsDto, PersistedGrantsApiDto>(MemberList.Destination);
        CreateMap<PersistedGrantsDto, PersistedGrantSubjectsApiDto>(MemberList.Destination);
    }
}
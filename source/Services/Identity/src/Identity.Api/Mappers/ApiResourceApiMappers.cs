using AutoMapper;

namespace Identity.Api.Mappers;

public static class ApiResourceApiMappers
{
    static ApiResourceApiMappers()
    {
        Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ApiResourceApiMapperProfile>())
            .CreateMapper();
    }

    internal static IMapper Mapper { get; }

    public static T ToApiResourceApiModel<T>(this object source) => Mapper.Map<T>(source);
}
using Identity.Api.ExceptionHandling;

namespace Identity.Api.Resources;

public class ApiErrorResources : IApiErrorResources
{
    public virtual ApiError CannotSetId() => new()
    {
        Code = nameof(CannotSetId),
        Description = ApiErrorResource.CannotSetId
    };
}
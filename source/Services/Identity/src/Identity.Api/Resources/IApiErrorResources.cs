using Identity.Api.ExceptionHandling;

namespace Identity.Api.Resources;

public interface IApiErrorResources
{
    ApiError CannotSetId();
}
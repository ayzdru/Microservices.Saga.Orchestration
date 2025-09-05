using Identity.BusinessLogic.Dtos.Configuration;
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.ApiResource;

public class ApiResourceUpdatedEvent : AuditEvent
{
    public ApiResourceUpdatedEvent(ApiResourceDto originalApiResource, ApiResourceDto apiResource)
    {
        OriginalApiResource = originalApiResource;
        ApiResource = apiResource;
    }

    public ApiResourceDto OriginalApiResource { get; set; }
    public ApiResourceDto ApiResource { get; set; }
}
using Identity.BusinessLogic.Dtos.Configuration;
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.ApiResource;

public class ApiResourceDeletedEvent : AuditEvent
{
    public ApiResourceDeletedEvent(ApiResourceDto apiResource)
    {
        ApiResource = apiResource;
    }

    public ApiResourceDto ApiResource { get; set; }
}
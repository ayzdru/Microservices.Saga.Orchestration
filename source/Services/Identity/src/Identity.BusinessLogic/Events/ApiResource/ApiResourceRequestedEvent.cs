using Identity.BusinessLogic.Dtos.Configuration;
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.ApiResource;

public class ApiResourceRequestedEvent : AuditEvent
{
    public ApiResourceRequestedEvent(int apiResourceId, ApiResourceDto apiResource)
    {
        ApiResourceId = apiResourceId;
        ApiResource = apiResource;
    }

    public int ApiResourceId { get; set; }
    public ApiResourceDto ApiResource { get; set; }
}
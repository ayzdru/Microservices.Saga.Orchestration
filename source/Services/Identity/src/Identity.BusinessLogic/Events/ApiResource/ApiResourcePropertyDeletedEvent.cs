using Identity.BusinessLogic.Dtos.Configuration;
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.ApiResource;

public class ApiResourcePropertyDeletedEvent : AuditEvent
{
    public ApiResourcePropertyDeletedEvent(ApiResourcePropertiesDto apiResourceProperty)
    {
        ApiResourceProperty = apiResourceProperty;
    }

    public ApiResourcePropertiesDto ApiResourceProperty { get; set; }
}
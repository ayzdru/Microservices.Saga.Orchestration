using Identity.BusinessLogic.Dtos.Configuration;
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.ApiScope;

public class ApiScopePropertyDeletedEvent : AuditEvent
{
    public ApiScopePropertyDeletedEvent(ApiScopePropertiesDto apiScopeProperty)
    {
        ApiScopeProperty = apiScopeProperty;
    }

    public ApiScopePropertiesDto ApiScopeProperty { get; set; }
}
using Identity.BusinessLogic.Dtos.Configuration;
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.ApiScope;

public class ApiScopeRequestedEvent : AuditEvent
{
    public ApiScopeRequestedEvent(ApiScopeDto apiScopes)
    {
        ApiScopes = apiScopes;
    }

    public ApiScopeDto ApiScopes { get; set; }
}
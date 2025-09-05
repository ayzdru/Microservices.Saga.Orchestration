using Identity.BusinessLogic.Dtos.Configuration;
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.ApiScope;

public class ApiScopeAddedEvent : AuditEvent
{
    public ApiScopeAddedEvent(ApiScopeDto apiScope)
    {
        ApiScope = apiScope;
    }

    public ApiScopeDto ApiScope { get; set; }
}
using Identity.BusinessLogic.Dtos.Configuration;
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.ApiScope;

public class ApiScopeUpdatedEvent : AuditEvent
{
    public ApiScopeUpdatedEvent(ApiScopeDto originalApiScope, ApiScopeDto apiScope)
    {
        OriginalApiScope = originalApiScope;
        ApiScope = apiScope;
    }

    public ApiScopeDto OriginalApiScope { get; set; }
    public ApiScopeDto ApiScope { get; set; }
}
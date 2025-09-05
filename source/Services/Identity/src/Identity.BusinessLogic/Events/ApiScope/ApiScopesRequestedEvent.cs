using Identity.BusinessLogic.Dtos.Configuration;
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.ApiScope;

public class ApiScopesRequestedEvent : AuditEvent
{
    public ApiScopesRequestedEvent(ApiScopesDto apiScope)
    {
        ApiScope = apiScope;
    }

    public ApiScopesDto ApiScope { get; set; }
}
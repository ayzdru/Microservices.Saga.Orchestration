using Identity.BusinessLogic.Dtos.Configuration;
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.ApiScope;

public class ApiScopePropertiesRequestedEvent : AuditEvent
{
    public ApiScopePropertiesRequestedEvent(int apiScopeId, ApiScopePropertiesDto apiScopeProperties)
    {
        ApiScopeId = apiScopeId;
        ApiResourceProperties = apiScopeProperties;
    }

    public int ApiScopeId { get; set; }
    public ApiScopePropertiesDto ApiResourceProperties { get; }
}
using Identity.BusinessLogic.Dtos.Configuration;
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.IdentityResource;

public class IdentityResourcePropertiesRequestedEvent : AuditEvent
{
    public IdentityResourcePropertiesRequestedEvent(IdentityResourcePropertiesDto identityResourceProperties)
    {
        IdentityResourceProperties = identityResourceProperties;
    }

    public IdentityResourcePropertiesDto IdentityResourceProperties { get; set; }
}
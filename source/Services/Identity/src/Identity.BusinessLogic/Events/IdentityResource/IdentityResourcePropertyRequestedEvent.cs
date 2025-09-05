using Identity.BusinessLogic.Dtos.Configuration;
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.IdentityResource;

public class IdentityResourcePropertyRequestedEvent : AuditEvent
{
    public IdentityResourcePropertyRequestedEvent(IdentityResourcePropertiesDto identityResourceProperties)
    {
        IdentityResourceProperties = identityResourceProperties;
    }

    public IdentityResourcePropertiesDto IdentityResourceProperties { get; set; }
}
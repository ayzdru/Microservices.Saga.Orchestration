using Identity.BusinessLogic.Dtos.Configuration;
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.IdentityResource;

public class IdentityResourcePropertyAddedEvent : AuditEvent
{
    public IdentityResourcePropertyAddedEvent(IdentityResourcePropertiesDto identityResourceProperty)
    {
        IdentityResourceProperty = identityResourceProperty;
    }

    public IdentityResourcePropertiesDto IdentityResourceProperty { get; set; }
}
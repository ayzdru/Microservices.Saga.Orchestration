using Identity.BusinessLogic.Dtos.Configuration;
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.IdentityResource;

public class IdentityResourcePropertyDeletedEvent : AuditEvent
{
    public IdentityResourcePropertyDeletedEvent(IdentityResourcePropertiesDto identityResourceProperty)
    {
        IdentityResourceProperty = identityResourceProperty;
    }

    public IdentityResourcePropertiesDto IdentityResourceProperty { get; set; }
}
using Identity.BusinessLogic.Dtos.Configuration;
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.IdentityResource;

public class IdentityResourceUpdatedEvent : AuditEvent
{
    public IdentityResourceUpdatedEvent(IdentityResourceDto originalIdentityResource,
        IdentityResourceDto identityResource)
    {
        OriginalIdentityResource = originalIdentityResource;
        IdentityResource = identityResource;
    }

    public IdentityResourceDto OriginalIdentityResource { get; set; }
    public IdentityResourceDto IdentityResource { get; set; }
}
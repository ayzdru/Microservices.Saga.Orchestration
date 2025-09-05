using Identity.BusinessLogic.Dtos.Configuration;
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.IdentityResource;

public class IdentityResourceDeletedEvent : AuditEvent
{
    public IdentityResourceDeletedEvent(IdentityResourceDto identityResource)
    {
        IdentityResource = identityResource;
    }

    public IdentityResourceDto IdentityResource { get; set; }
}
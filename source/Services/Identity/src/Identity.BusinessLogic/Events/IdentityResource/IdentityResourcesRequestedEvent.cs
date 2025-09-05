using Identity.BusinessLogic.Dtos.Configuration;
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.IdentityResource;

public class IdentityResourcesRequestedEvent : AuditEvent
{
    public IdentityResourcesRequestedEvent(IdentityResourcesDto identityResources)
    {
        IdentityResources = identityResources;
    }

    public IdentityResourcesDto IdentityResources { get; }
}
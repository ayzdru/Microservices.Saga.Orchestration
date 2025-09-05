using Identity.BusinessLogic.Identity.Dtos.Grant;
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Identity.Events.PersistedGrant;

public class PersistedGrantsIdentityByUserRequestedEvent : AuditEvent
{
    public PersistedGrantsIdentityByUserRequestedEvent(PersistedGrantsDto persistedGrants)
    {
        PersistedGrants = persistedGrants;
    }

    public PersistedGrantsDto PersistedGrants { get; set; }
}
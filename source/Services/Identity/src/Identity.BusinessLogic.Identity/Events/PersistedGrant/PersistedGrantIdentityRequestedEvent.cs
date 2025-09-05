using Identity.BusinessLogic.Identity.Dtos.Grant;
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Identity.Events.PersistedGrant;

public class PersistedGrantIdentityRequestedEvent : AuditEvent
{
    public PersistedGrantIdentityRequestedEvent(PersistedGrantDto persistedGrant)
    {
        PersistedGrant = persistedGrant;
    }

    public PersistedGrantDto PersistedGrant { get; set; }
}
using Identity.BusinessLogic.Dtos.Grant;
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.PersistedGrant;

public class PersistedGrantRequestedEvent : AuditEvent
{
    public PersistedGrantRequestedEvent(PersistedGrantDto persistedGrant)
    {
        PersistedGrant = persistedGrant;
    }

    public PersistedGrantDto PersistedGrant { get; set; }
}
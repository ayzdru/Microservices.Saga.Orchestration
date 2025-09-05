using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.PersistedGrant;

public class PersistedGrantDeletedEvent : AuditEvent
{
    public PersistedGrantDeletedEvent(string persistedGrantKey)
    {
        PersistedGrantKey = persistedGrantKey;
    }

    public string PersistedGrantKey { get; set; }
}
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Identity.Events.PersistedGrant;

public class PersistedGrantsIdentityDeletedEvent : AuditEvent
{
    public PersistedGrantsIdentityDeletedEvent(string userId)
    {
        UserId = userId;
    }

    public string UserId { get; set; }
}
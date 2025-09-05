using Identity.BusinessLogic.Dtos.Grant;
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.PersistedGrant;

public class PersistedGrantsByUserRequestedEvent : AuditEvent
{
    public PersistedGrantsByUserRequestedEvent(PersistedGrantsDto persistedGrants)
    {
        PersistedGrants = persistedGrants;
    }

    public PersistedGrantsDto PersistedGrants { get; set; }
}
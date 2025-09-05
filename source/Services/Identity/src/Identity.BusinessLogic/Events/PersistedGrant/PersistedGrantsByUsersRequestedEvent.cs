using Identity.BusinessLogic.Dtos.Grant;
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.PersistedGrant;

public class PersistedGrantsByUsersRequestedEvent : AuditEvent
{
    public PersistedGrantsByUsersRequestedEvent(PersistedGrantsDto persistedGrants)
    {
        PersistedGrants = persistedGrants;
    }

    public PersistedGrantsDto PersistedGrants { get; set; }
}
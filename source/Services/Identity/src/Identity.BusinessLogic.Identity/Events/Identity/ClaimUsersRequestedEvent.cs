using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Identity.Events.Identity;

public class ClaimUsersRequestedEvent<TUsersDto> : AuditEvent
{
    public ClaimUsersRequestedEvent(TUsersDto users)
    {
        Users = users;
    }

    public TUsersDto Users { get; set; }
}
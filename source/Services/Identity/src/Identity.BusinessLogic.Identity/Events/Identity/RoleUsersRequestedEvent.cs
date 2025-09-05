using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Identity.Events.Identity;

public class RoleUsersRequestedEvent<TUsersDto> : AuditEvent
{
    public RoleUsersRequestedEvent(TUsersDto users)
    {
        Users = users;
    }

    public TUsersDto Users { get; set; }
}
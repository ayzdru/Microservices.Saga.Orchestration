using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Identity.Events.Identity;

public class UserRoleDeletedEvent<TUserRolesDto> : AuditEvent
{
    public UserRoleDeletedEvent(TUserRolesDto role)
    {
        Role = role;
    }

    public TUserRolesDto Role { get; set; }
}
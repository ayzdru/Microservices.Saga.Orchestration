using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Identity.Events.Identity;

public class UserRoleSavedEvent<TUserRolesDto> : AuditEvent
{
    public UserRoleSavedEvent(TUserRolesDto role)
    {
        Role = role;
    }

    public TUserRolesDto Role { get; set; }
}
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Identity.Events.Identity;

public class UserRolesRequestedEvent<TUserRolesDto> : AuditEvent
{
    public UserRolesRequestedEvent(TUserRolesDto roles)
    {
        Roles = roles;
    }

    public TUserRolesDto Roles { get; set; }
}
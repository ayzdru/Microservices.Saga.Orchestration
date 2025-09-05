using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Identity.Events.Identity;

public class RoleAddedEvent<TRoleDto> : AuditEvent
{
    public RoleAddedEvent(TRoleDto role)
    {
        Role = role;
    }

    public TRoleDto Role { get; set; }
}
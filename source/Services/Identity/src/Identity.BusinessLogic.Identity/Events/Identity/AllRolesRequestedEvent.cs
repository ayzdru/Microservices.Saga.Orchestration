using System.Collections.Generic;
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Identity.Events.Identity;

public class AllRolesRequestedEvent<TRoleDto> : AuditEvent
{
    public AllRolesRequestedEvent(List<TRoleDto> roles)
    {
        Roles = roles;
    }

    public List<TRoleDto> Roles { get; set; }
}
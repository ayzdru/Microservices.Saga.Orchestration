using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Identity.Events.Identity;

public class RoleClaimsDeletedEvent<TRoleClaimsDto> : AuditEvent
{
    public RoleClaimsDeletedEvent(TRoleClaimsDto roleClaim)
    {
        RoleClaim = roleClaim;
    }

    public TRoleClaimsDto RoleClaim { get; set; }
}
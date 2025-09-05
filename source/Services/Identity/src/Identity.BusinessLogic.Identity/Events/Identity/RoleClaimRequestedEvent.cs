using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Identity.Events.Identity;

public class RoleClaimRequestedEvent<TRoleClaimsDto> : AuditEvent
{
    public RoleClaimRequestedEvent(TRoleClaimsDto roleClaim)
    {
        RoleClaim = roleClaim;
    }

    public TRoleClaimsDto RoleClaim { get; set; }
}
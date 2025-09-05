using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Identity.Events.Identity;

public class RoleClaimsSavedEvent<TRoleClaimsDto> : AuditEvent
{
    public RoleClaimsSavedEvent(TRoleClaimsDto claims)
    {
        Claims = claims;
    }

    public TRoleClaimsDto Claims { get; set; }
}
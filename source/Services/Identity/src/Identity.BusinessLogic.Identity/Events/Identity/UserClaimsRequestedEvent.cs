using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Identity.Events.Identity;

public class UserClaimsRequestedEvent<TUserClaimsDto> : AuditEvent
{
    public UserClaimsRequestedEvent(TUserClaimsDto claims)
    {
        Claims = claims;
    }

    public TUserClaimsDto Claims { get; set; }
}
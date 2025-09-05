using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Identity.Events.Identity;

public class UserClaimRequestedEvent<TUserClaimsDto> : AuditEvent
{
    public UserClaimRequestedEvent(TUserClaimsDto userClaims)
    {
        UserClaims = userClaims;
    }

    public TUserClaimsDto UserClaims { get; set; }
}
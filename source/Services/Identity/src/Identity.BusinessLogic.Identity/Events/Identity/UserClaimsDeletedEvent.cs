using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Identity.Events.Identity;

public class UserClaimsDeletedEvent<TUserClaimsDto> : AuditEvent
{
    public UserClaimsDeletedEvent(TUserClaimsDto claim)
    {
        Claim = claim;
    }

    public TUserClaimsDto Claim { get; set; }
}
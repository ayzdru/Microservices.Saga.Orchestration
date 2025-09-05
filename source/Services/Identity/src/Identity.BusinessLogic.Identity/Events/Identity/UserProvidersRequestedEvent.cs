using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Identity.Events.Identity;

public class UserProvidersRequestedEvent<TUserProvidersDto> : AuditEvent
{
    public UserProvidersRequestedEvent(TUserProvidersDto providers)
    {
        Providers = providers;
    }

    public TUserProvidersDto Providers { get; set; }
}
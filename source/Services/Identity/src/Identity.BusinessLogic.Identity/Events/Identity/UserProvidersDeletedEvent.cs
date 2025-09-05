using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Identity.Events.Identity;

public class UserProvidersDeletedEvent<TUserProviderDto> : AuditEvent
{
    public UserProvidersDeletedEvent(TUserProviderDto provider)
    {
        Provider = provider;
    }

    public TUserProviderDto Provider { get; set; }
}
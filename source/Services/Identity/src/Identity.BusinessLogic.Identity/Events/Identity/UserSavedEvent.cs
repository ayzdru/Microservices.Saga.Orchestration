using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Identity.Events.Identity;

public class UserSavedEvent<TUserDto> : AuditEvent
{
    public UserSavedEvent(TUserDto user)
    {
        User = user;
    }

    public TUserDto User { get; set; }
}
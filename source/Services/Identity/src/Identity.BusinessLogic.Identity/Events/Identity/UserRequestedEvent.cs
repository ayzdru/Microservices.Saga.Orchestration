using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Identity.Events.Identity;

public class UserRequestedEvent<TUserDto> : AuditEvent
{
    public UserRequestedEvent(TUserDto userDto)
    {
        UserDto = userDto;
    }

    public TUserDto UserDto { get; set; }
}
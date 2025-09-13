
namespace BuildingBlocks.EventBus.Events.User;

public class UserRegisteredEvent
{
    public Guid UserId { get;set; }
    public string UserName { get;set; }
    public string Password { get;set; }
    public string Email { get;set; }
}
namespace BuildingBlocks.EventBus.Events.Identity;

public class UserCreatedEvent
{
    public string UserId { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
}
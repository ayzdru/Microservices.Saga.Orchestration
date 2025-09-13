namespace BuildingBlocks.EventBus.Events.Order;

public class OrderCompletedEvent
{
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; }
}
using BuildingBlocks.EventBus.Interfaces.Order;

namespace BuildingBlocks.EventBus.Events.Order;

public class OrderCompletedEvent : IOrderCompletedEvent
{
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; }
}
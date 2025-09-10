using BuildingBlocks.EventBus.Interfaces.Order;

namespace BuildingBlocks.EventBus.Events.Order;

public class OrderFailedEvent : IOrderFailedEvent
{
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public string ErrorMessage { get; set; }
}
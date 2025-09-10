using BuildingBlocks.EventBus.Interfaces.Order;
using BuildingBlocks.EventBus.Models.Order;

namespace BuildingBlocks.EventBus.Events.Order;

public class OrderCreatedEvent : IOrderCreatedEvent
{
    public Guid CorrelationId { get; set; }
    public List<OrderItem> OrderItems { get; set; }
}
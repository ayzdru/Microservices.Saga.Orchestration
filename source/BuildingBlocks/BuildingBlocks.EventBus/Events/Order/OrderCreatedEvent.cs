using BuildingBlocks.EventBus.Models.Order;

namespace BuildingBlocks.EventBus.Events.Order;

public class OrderCreatedEvent
{
    public Guid CorrelationId { get; set; }
    public List<OrderItem> OrderItems { get; set; }
}
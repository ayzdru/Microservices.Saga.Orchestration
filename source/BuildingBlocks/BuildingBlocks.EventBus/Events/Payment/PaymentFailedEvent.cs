using BuildingBlocks.EventBus.Models.Order;

namespace BuildingBlocks.EventBus.Events.Payment;

public class PaymentFailedEvent
{
    public Guid CorrelationId { get; set; }
    public List<OrderItem> OrderItems { get; set; }
    public string ErrorMessage { get; set; }
}
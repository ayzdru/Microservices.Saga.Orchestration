using BuildingBlocks.EventBus.Models.Order;

namespace BuildingBlocks.EventBus.Interfaces.Payment;

public interface IPaymentFailedEvent
{
    Guid CorrelationId { get; set; }
    public string ErrorMessage { get; set; }
    public List<OrderItem> OrderItems { get; set; }
}
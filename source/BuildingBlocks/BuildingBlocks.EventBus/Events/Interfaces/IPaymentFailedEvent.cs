
namespace EventBus.Events.Interfaces;

public interface IPaymentFailedEvent
{
    Guid CorrelationId { get; set; }
    public string ErrorMessage { get; set; }
    public List<OrderItem> OrderItemList { get; set; }
}
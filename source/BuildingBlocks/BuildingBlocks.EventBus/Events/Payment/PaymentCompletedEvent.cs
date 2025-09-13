
namespace BuildingBlocks.EventBus.Events.Payment;

public class PaymentCompletedEvent
{
    public Guid CorrelationId { get; set; }
}

namespace BuildingBlocks.EventBus.Events.Order;

public class OrderFailedEvent
{
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public string ErrorMessage { get; set; }
}
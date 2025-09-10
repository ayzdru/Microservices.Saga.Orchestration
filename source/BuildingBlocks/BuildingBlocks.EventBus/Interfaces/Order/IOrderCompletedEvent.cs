namespace BuildingBlocks.EventBus.Interfaces.Order;

public interface IOrderCompletedEvent
{
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; }
}
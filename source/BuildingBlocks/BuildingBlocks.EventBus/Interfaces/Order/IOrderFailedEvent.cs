namespace BuildingBlocks.EventBus.Interfaces.Order;

public interface IOrderFailedEvent
{
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public string ErrorMessage { get; set; }
}
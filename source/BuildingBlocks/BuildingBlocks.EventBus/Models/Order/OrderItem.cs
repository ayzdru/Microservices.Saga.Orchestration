namespace BuildingBlocks.EventBus.Models.Order;

public class OrderItem
{
    public Guid ProductId { get; set; }
    public decimal Price { get; set; }
    public int Count { get; set; }
}
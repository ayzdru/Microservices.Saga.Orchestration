using BuildingBlocks.Core.Common;

namespace Order.Core.Entities;

public class OrderItem : BaseEntity
{
    public Guid OrderId { get; private set; }
    public Order Order { get; private set; }
    public Guid ProductId { get; private set; }
    public decimal Price { get; private set; }
    public int Count { get; private set; }
    public OrderItem(Guid orderId, Guid productId, decimal price, int count)
    {
        OrderId = orderId;
        ProductId = productId;
        Price = price;
        Count = count;
    }
    public OrderItem(Guid productId, decimal price, int count)
    {
        ProductId = productId;
        Price = price;
        Count = count;
    }
}
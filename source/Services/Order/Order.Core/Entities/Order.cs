using BuildingBlocks.Core.Common;
using Order.Core.Enums;

namespace Order.Core.Entities;

public class Order : BaseEntity
{    
    public Guid UserId { get; private set; }
    public OrderStatus Status { get; private set; }
    public string ErrorMessage { get; private set; }
    private readonly List<OrderItem> _orderItems = new List<OrderItem>();
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();
    public Order(Guid userId, OrderStatus status, string errorMessage)
    {
        UserId = userId;
        Status = status;
        ErrorMessage = errorMessage;
    }
    public Order(Guid userId, OrderStatus status, List<OrderItem> orderItems)
    {
        UserId = userId;
        Status = status;
        _orderItems = orderItems;
    }
    public void ChangeStatus(OrderStatus newStatus, string? errorMessage = null)
    {
        Status = newStatus;
        ErrorMessage = errorMessage ?? string.Empty;
    }
}
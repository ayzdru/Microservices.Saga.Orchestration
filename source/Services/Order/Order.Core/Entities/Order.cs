using BuildingBlocks.Core.Common;
using Order.Core.Enums;

namespace Order.Core.Entities;

public class Order : BaseEntity
{
    public Order()
    {
        OrderItemList = new List<OrderItem>();
    }
    public int Id { get; set; }
    public string CustomerId { get; set; }
    public string PaymentAccountId { get; set; }
    public OrderStatus Status { get; set; }
    public string ErrorMessage { get; set; }
    
    public virtual List<OrderItem> OrderItemList { get; set; } 
}
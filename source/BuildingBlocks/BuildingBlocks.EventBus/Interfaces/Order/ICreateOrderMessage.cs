using BuildingBlocks.EventBus.Models.Order;
using System.Collections.Generic;

namespace BuildingBlocks.EventBus.Interfaces.Order;

public interface ICreateOrderMessage
{
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public decimal TotalPrice { get; set; }
    public List<OrderItem> OrderItems { get; set; }
}
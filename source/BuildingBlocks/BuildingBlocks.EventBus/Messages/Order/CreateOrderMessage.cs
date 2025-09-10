using System.Collections.Generic;
using BuildingBlocks.EventBus.Interfaces.Order;
using BuildingBlocks.EventBus.Models.Order;

namespace BuildingBlocks.EventBus.Messages.Order;

public class CreateOrderMessage : ICreateOrderMessage
{
    public CreateOrderMessage()
    {
        OrderItems = new List<OrderItem>();
    }

    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public decimal TotalPrice { get; set; }

    public List<OrderItem> OrderItems { get; set; }
}
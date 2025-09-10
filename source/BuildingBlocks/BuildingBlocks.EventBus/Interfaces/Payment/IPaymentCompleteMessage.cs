using BuildingBlocks.EventBus.Models.Order;
using System;
using System.Collections.Generic;

namespace BuildingBlocks.EventBus.Interfaces.Payment;

public interface IPaymentCompleteMessage
{
    Guid CorrelationId { get; set; }
    public Guid UserId { get; set; }
    public decimal TotalPrice { get; set; }
    public List<OrderItem> OrderItems { get; set; }

}
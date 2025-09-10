using System;
using System.Collections.Generic;
using EventBus.Events;

namespace EventBus.Messages.Interfaces;

public interface ICompletePaymentMessage
{
    Guid CorrelationId { get; set; }
    public string CustomerId { get; set; }
    public decimal TotalPrice { get; set; }
    public List<OrderItem> OrderItemList { get; set; }

}
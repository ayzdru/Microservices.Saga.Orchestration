using Orchestration.Core.Models.Order;
using System;
using System.Collections.Generic;

namespace Microservices.Saga.Orchestration.Shared.Events.Order
{
    public record OrderInitializedEvent
    {
        public Guid CorrelationId { get; init; }
        public List<OrderItem> OrderItems { get; set; }
    }
}

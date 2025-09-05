using MassTransit;
using Microservices.Saga.Orchestration.Shared.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Saga.Orchestration.Shared.Models.Payment
{
    public record PaymentFailedEvent
    {
        public Guid CorrelationId { get; init; }
        public string Message { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}

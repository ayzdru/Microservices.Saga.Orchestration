using MassTransit;
using Microservices.Saga.Orchestration.Shared.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Saga.Orchestration.Shared.Models.Payment
{
    public record PaymentStartedEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; init; }
        public decimal TotalPrice { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Saga.Orchestration.Shared.Events.Payment
{
    public record PaymentSuccessEvent
    {
        public Guid CorrelationId { get; init; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Saga.Orchestration.Shared.Events.Product
{
    public record ProductStockReservationFailedEvent
    {
        public Guid CorrelationId { get; init; }
        public string Message { get; set; }
    }
}

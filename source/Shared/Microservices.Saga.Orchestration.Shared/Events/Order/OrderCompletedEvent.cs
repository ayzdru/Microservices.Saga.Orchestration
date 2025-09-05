using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Saga.Orchestration.Shared.Events.Order
{
    public record OrderCompletedEvent
    {
        public Guid OrderId { get; set; }
    }
}

using MassTransit;
using Microservices.Saga.Orchestration.Shared.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Saga.Orchestration.Shared.Models.Product
{
    public class ProductStockReservationSuccessEvent : CorrelatedBy<Guid>
    {
		public Guid CorrelationId { get; init; }
		public List<OrderItem> OrderItems { get; set; }
    }
}

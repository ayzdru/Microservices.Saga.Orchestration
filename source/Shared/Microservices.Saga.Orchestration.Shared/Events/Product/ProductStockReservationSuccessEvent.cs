using Orchestration.Core.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Saga.Orchestration.Shared.Events.Product
{
    public class ProductStockReservationSuccessEvent
    {
		public Guid CorrelationId { get; init; }
		public List<OrderItem> OrderItems { get; set; }
    }
}

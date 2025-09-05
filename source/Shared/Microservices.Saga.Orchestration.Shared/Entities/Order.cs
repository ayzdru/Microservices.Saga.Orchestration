using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Saga.Orchestration.Shared.Entities
{
    public class Order
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }

        public decimal TotalPrice { get; set; }
    }
}

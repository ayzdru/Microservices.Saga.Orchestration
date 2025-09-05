using Microservices.Saga.Orchestration.Shared.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Saga.Orchestration.Shared.Models.Product
{
    public class StockRollback
    {
        public List<OrderItem> OrderItems { get; set; }
    }
}

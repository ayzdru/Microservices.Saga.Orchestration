using Orchestration.Core.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orchestration.Core.Models.Product
{
    public class StockRollback
    {
        public List<OrderItem> OrderItems { get; set; }
    }
}

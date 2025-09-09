using Orchestration.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orchestration.Core.Entities
{
    public class OrderState : BaseEntity
    {
        public Guid OrderId { get; private set; }
        public DateTime OrderDate { get; private set; }

        public decimal TotalPrice { get; private set; }
        public OrderState(Guid orderId, DateTime orderDate, decimal totalPrice)
        {
            OrderId = orderId;
            OrderDate = orderDate;
            TotalPrice = totalPrice;
        }
    }
}

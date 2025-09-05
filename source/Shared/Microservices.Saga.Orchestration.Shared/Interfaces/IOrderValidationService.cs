using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.Saga.Orchestration.Shared.Interfaces
{
    public interface IOrderValidationService
    {
        Task ValidateOrder(Guid orderId);
    }
}

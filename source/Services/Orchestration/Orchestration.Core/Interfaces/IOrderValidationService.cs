using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orchestration.Core.Interfaces
{
    public interface IOrderValidationService
    {
        Task ValidateOrder(Guid orderId);
    }
}

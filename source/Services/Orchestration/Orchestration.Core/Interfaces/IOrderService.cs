
using Orchestration.Core.Entities;
using Orchestration.Core.Models.Order;

namespace Microservices.Saga.Orchestration.Shared.Interfaces
{
    public interface IOrderService
    {
        Task<OrderState> SubmitOrder(List<OrderItem> orderItems);
    }
}
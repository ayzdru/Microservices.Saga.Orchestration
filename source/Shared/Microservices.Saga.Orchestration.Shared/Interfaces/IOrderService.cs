using Microservices.Saga.Orchestration.Shared.Entities;
using Microservices.Saga.Orchestration.Shared.Models.Order;

namespace Microservices.Saga.Orchestration.Shared.Interfaces
{
    public interface IOrderService
    {
        Task<OrderState> SubmitOrder(List<OrderItem> orderItems);
    }
}
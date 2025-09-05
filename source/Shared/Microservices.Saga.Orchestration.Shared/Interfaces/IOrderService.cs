using Microservices.Saga.Orchestration.Shared.Entities;
using Microservices.Saga.Orchestration.Shared.Models.Order;

namespace Microservices.Saga.Orchestration.Shared.Interfaces
{
    public interface IOrderService
    {
        Task<Order> SubmitOrder(List<OrderItem> orderItems);
    }
}
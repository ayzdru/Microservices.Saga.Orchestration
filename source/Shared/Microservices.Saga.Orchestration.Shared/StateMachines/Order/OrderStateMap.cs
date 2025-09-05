using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservices.Saga.Orchestration.Shared.StateMachines.Order
{
    public class OrderStateMap : SagaClassMap<OrderState>
    {
        protected override void Configure(EntityTypeBuilder<OrderState> entity, ModelBuilder model)
        {
            entity.Property(x => x.CurrentState);
            entity.Property(x => x.CreatedDate);
            entity.Property(x => x.OrderId);
            entity.Property(x => x.UserId);
            entity.Property(x => x.TotalPrice);
        }
    }
}
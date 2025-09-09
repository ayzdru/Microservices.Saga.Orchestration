using MassTransit;
using Microservices.Saga.Orchestration.Shared.Events.Order;
using Microservices.Saga.Orchestration.Shared.Events.Payment;
using Microservices.Saga.Orchestration.Shared.Events.Product;
using Orchestration.Core.Models.Product;

namespace Orchestration.Infrastructure.StateMachines.Order
{
    public class OrderStateMachine : MassTransitStateMachine<OrderStateMachineInstance>
    {
        public Event<OrderStartedEvent> OrderStartedEvent { get; set; }
        public Event<ProductStockReservationSuccessEvent> ProductStockReservationSuccessEvent { get; set; }
        public Event<ProductStockReservationFailedEvent> ProductStockReservationFailedEvent { get; set; }
        public Event<PaymentSuccessEvent> PaymentSuccessEvent { get; set; }
        public Event<PaymentFailedEvent> PaymentFailedEvent { get; set; }

        public State OrderInitialized { get; set; }
        public State ProductStockReservationSuccess { get; set; }
        public State ProductStockReservationFailed { get; set; }
        public State PaymentSuccess { get; set; }
        public State PaymentFailed { get; set; }
        public OrderStateMachine()
        {
            InstanceState(instance => instance.CurrentState);

            Event(() => OrderStartedEvent,
                orderStateInstance => orderStateInstance.CorrelateBy<Guid>(database => database.OrderId, @event => @event.Message.OrderId)
                .SelectId(e => Guid.NewGuid()));

            Event(() => ProductStockReservationSuccessEvent,
                orderStateInstance => orderStateInstance.CorrelateById(@event => @event.Message.CorrelationId));

            Event(() => ProductStockReservationFailedEvent,
                orderStateInstance => orderStateInstance.CorrelateById(@event => @event.Message.CorrelationId));

            Event(() => PaymentSuccessEvent,
                orderStateInstance => orderStateInstance.CorrelateById(@event => @event.Message.CorrelationId));

            Event(() => PaymentFailedEvent,
                orderStateInstance => orderStateInstance.CorrelateById(@event => @event.Message.CorrelationId));


            Initially(When(OrderStartedEvent)
                .Then(context =>
                {
                    context.Saga.OrderId = context.Message.OrderId;
                    context.Saga.UserId = context.Message.UserId;
                    context.Saga.TotalPrice = context.Message.TotalPrice;
                    context.Saga.CreatedDate = DateTime.UtcNow;
                })
                .TransitionTo(OrderInitialized)
                .Send(new Uri($"queue:{Constants.RabbitMQ.Product_OrderInitializedEventQueue}"),
                context => new OrderInitializedEvent()
                {
                    CorrelationId = context.Saga.CorrelationId,
                    OrderItems = context.Message.OrderItems
                }));

            During(OrderInitialized,
                When(ProductStockReservationSuccessEvent)
                .TransitionTo(ProductStockReservationSuccess)
                .Send(new Uri($"queue:{Constants.RabbitMQ.Payment_StartedEventQueue}"),
                context => new PaymentStartedEvent()
                {
                    CorrelationId= context.Saga.CorrelationId,
                    TotalPrice = context.Saga.TotalPrice,
                    OrderItems = context.Message.OrderItems
                }),
                When(ProductStockReservationFailedEvent)
                .TransitionTo(ProductStockReservationFailed)
                .Send(new Uri($"queue:{Constants.RabbitMQ.Order_OrderFailedEventQueue}"),
                context => new OrderFailedEvent
                {
                    OrderId = context.Saga.OrderId,
                    Message = context.Message.Message
                }));

            During(ProductStockReservationSuccess,
                When(PaymentSuccessEvent)
                .TransitionTo(PaymentSuccess)
                .Send(new Uri($"queue:{Constants.RabbitMQ.Order_OrderCompletedEventQueue}"),
                context => new OrderCompletedEvent
                {
                    OrderId = context.Saga.OrderId
                })
                .Finalize(),
                When(PaymentFailedEvent)
                .TransitionTo(PaymentFailed)
                .Send(new Uri($"queue:{Constants.RabbitMQ.Order_OrderFailedEventQueue}"),
                context => new OrderFailedEvent
                {
                    OrderId = context.Saga.OrderId,
                    Message = context.Message.Message
                })
                .Send(new Uri($"queue:{Constants.RabbitMQ.Product_RollbackMessageQueue}"),
                context => new StockRollback
                {
                    OrderItems = context.Message.OrderItems
                }));

            SetCompletedWhenFinalized();
        }


    }
}
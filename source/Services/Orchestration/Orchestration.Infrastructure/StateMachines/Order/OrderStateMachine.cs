using BuildingBlocks.EventBus.Events.Order;
using BuildingBlocks.EventBus.Events.Payment;
using BuildingBlocks.EventBus.Events.Product;
using BuildingBlocks.EventBus.Messages.Order;
using BuildingBlocks.EventBus.Messages.Payment;
using BuildingBlocks.EventBus.Messages.Product;
using MassTransit;
using Microsoft.Extensions.Logging;


namespace Orchestration.Infrastructure.StateMachines.Order;

public class OrderStateMachine : MassTransitStateMachine<OrderState>
{
    private readonly ILogger _logger;

    // Commands
    private Event<CreateOrderMessage> CreateOrderMessage { get; set; }

    // Events
    public Event<StockReservedEvent> StockReservedEvent { get; set; }
    public Event<StockReservationFailedEvent> StockReservationFailedEvent { get; set; }
    public Event<PaymentCompletedEvent> PaymentCompletedEvent { get; set; }
    public Event<PaymentFailedEvent> PaymentFailedEvent { get; set; }

    // States
    public State OrderCreated { get; set; }
    public State StockReserved { get; set; }
    public State StockReservationFailed { get; set; }
    public State PaymentCompleted { get; set; }
    public State PaymentFailed { get; set; }

    public OrderStateMachine(ILogger<OrderStateMachine> logger)
    {
        _logger = logger; 
        InstanceState(x => x.CurrentState);

        Event(() => CreateOrderMessage, y => y.CorrelateBy<Guid>(x => x.OrderId, z => z.Message.OrderId)
            .SelectId(context => Guid.NewGuid()));
        Event(() => StockReservedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));
        Event(() => StockReservationFailedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));
        Event(() => PaymentCompletedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));

        Initially(
            When(CreateOrderMessage)
                .Then(context => { _logger.LogInformation($"CorrelationId {context.Saga.CorrelationId} - CreateOrderMessage received in OrderStateMachine: {context.Saga}"); })
                .Then(context =>
                {
                    context.Saga.UserId = context.Message.UserId;
                    context.Saga.OrderId = context.Message.OrderId;
                    context.Saga.CreatedDate = DateTime.UtcNow;
                    context.Saga.TotalPrice = context.Message.TotalPrice;
                })
                .Publish(
                    context => new OrderCreatedEvent
                    {
                        CorrelationId = context.Saga.CorrelationId,
                        OrderItems = context.Message.OrderItems
                    })
                .TransitionTo(OrderCreated)
                .Then(context => { _logger.LogInformation($"CorrelationId {context.Saga.CorrelationId} - OrderCreatedEvent published in OrderStateMachine: {context.Saga}"); }));

        During(OrderCreated,
            When(StockReservedEvent)
                .Then(context => { _logger.LogInformation($"CorrelationId {context.Saga.CorrelationId} - StockReservedEvent received in OrderStateMachine: {context.Saga}"); })
                .TransitionTo(StockReserved)
                .Send(new Uri($"queue:{EventBusConstants.Queues.CompletePaymentMessageQueueName}"),
                    context => new PaymentCompleteMessage
                    {
                        CorrelationId = context.Saga.CorrelationId,
                        TotalPrice = context.Saga.TotalPrice,
                        UserId = context.Saga.UserId,
                        OrderItems = context.Message.OrderItems
                    })
                .Then(context => { _logger.LogInformation($"CorrelationId {context.Saga.CorrelationId} - CompletePaymentMessage sent in OrderStateMachine: {context.Saga}"); }),
            When(StockReservationFailedEvent)
                .Then(context => { _logger.LogInformation($"CorrelationId {context.Saga.CorrelationId} - StockReservationFailedEvent received in OrderStateMachine: {context.Saga}"); })
                .TransitionTo(StockReservationFailed)
                .Publish(
                    context => new OrderFailedEvent
                    {
                        OrderId = context.Saga.OrderId,
                        UserId = context.Saga.UserId,
                        ErrorMessage = context.Message.ErrorMessage
                    })
                .Then(context => { _logger.LogInformation($"CorrelationId {context.Saga.CorrelationId} - OrderFailedEvent published in OrderStateMachine: {context.Saga}"); })
        );

        During(StockReserved,
            When(PaymentCompletedEvent)
                .Then(context => { _logger.LogInformation($"CorrelationId {context.Saga.CorrelationId} - PaymentCompletedEvent received in OrderStateMachine: {context.Saga}"); })
                .TransitionTo(PaymentCompleted)
                .Publish(
                    context => new OrderCompletedEvent
                    {
                        OrderId = context.Saga.OrderId,
                        UserId = context.Saga.UserId
                    })
                .Then(context => { _logger.LogInformation($"CorrelationId {context.Saga.CorrelationId} - OrderCompletedEvent published in OrderStateMachine: {context.Saga}"); })
                .Finalize(),
            When(PaymentFailedEvent)
                .Then(context => { _logger.LogInformation($"CorrelationId {context.Saga.CorrelationId} - PaymentFailedEvent received in OrderStateMachine: {context.Saga}"); })
                .Publish(context => new OrderFailedEvent
                {
                    OrderId = context.Saga.OrderId,
                    UserId = context.Saga.UserId,
                    ErrorMessage = context.Message.ErrorMessage
                })
                .Then(context => { _logger.LogInformation($"CorrelationId {context.Saga.CorrelationId} - OrderFailedEvent published in OrderStateMachine: {context.Saga}"); })
                .Send(new Uri($"queue:{EventBusConstants.Queues.StockRollBackMessageQueueName}"),
                    context => new StockRollbackMessage
                    {
                        OrderItems = context.Message.OrderItems
                    })
                .Then(context => { _logger.LogInformation($"CorrelationId {context.Saga.CorrelationId} - StockRollbackMessage sent in OrderStateMachine: {context.Saga}"); })
                .TransitionTo(PaymentFailed)
        );
    }
}

using BuildingBlocks.EventBus.Events.Payment;
using BuildingBlocks.EventBus.Interfaces.Payment;
using BuildingBlocks.EventBus.Messages.Payment;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Payment.Infrastructure.Consumers.Events;

public class CompletePaymentMessageConsumer : IConsumer<IPaymentCompleteMessage>
{
    private readonly ILogger<CompletePaymentMessageConsumer> _logger;

    private readonly IPublishEndpoint _publishEndpoint;

    public CompletePaymentMessageConsumer(ILogger<CompletePaymentMessageConsumer> logger, IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<IPaymentCompleteMessage> context)
    {
        // todo payment from stripe service
        var paymentSuccess = DateTime.UtcNow.Second % 2 == 0;

        // if (paymentSuccess)
        // {
            _logger.LogInformation("Payment successfull. {MessageTotalPrice}$ was withdrawn from user with UserId= {MessageUserId} and correlation Id={MessageCorrelationId}",
                context.Message.TotalPrice, context.Message.UserId, context.Message.CorrelationId);

            await _publishEndpoint.Publish(new PaymentCompletedEvent
            {
                CorrelationId = context.Message.CorrelationId
            });

            return;
        // }
        //
        // _logger.LogInformation("Payment failed. {MessageTotalPrice}$ was not withdrawn from user with Id={MessageCustomerId} and correlation Id={MessageCorrelationId}",
        //     context.Message.TotalPrice, context.Message.CustomerId, context.Message.CorrelationId);
        //
        // await _publishEndpoint.Publish(new PaymentFailedEvent
        // {
        //     CorrelationId = context.Message.CorrelationId,
        //     ErrorMessage = "Payment failed", 
        //     OrderItemList = context.Message.OrderItemList
        // });
    }
}
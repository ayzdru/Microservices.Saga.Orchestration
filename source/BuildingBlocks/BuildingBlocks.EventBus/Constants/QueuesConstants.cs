public partial class EventBusConstants
{
    public static class Queues
    {
        // events
        public const string OrderCreatedEventQueueName = "order-created-queue";
        public const string OrderCompletedEventQueueName = "order-completed-queue";
        public const string OrderFailedEventQueueName = "order-failed-queue";

        public const string ProductUserRegisteredEventQueueName = "product-user-registered-queue";
        public const string OrderUserRegisteredEventQueueName = "order-user-registered-queue";
        public const string PaymentUserRegisteredEventQueueName = "payment-user-registered-queue";
        public const string OrchestrationUserRegisteredEventQueueName = "orchestration-user-registered-queue";

        // messages
        public const string CreateOrderMessageQueueName = "create-order-message-queue";
        public const string CompletePaymentMessageQueueName = "complete-payment-message-queue";
        public const string StockRollBackMessageQueueName = "stock-rollback-message-queue";
    }
}

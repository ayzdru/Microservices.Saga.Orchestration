namespace Microservices.Saga.Orchestration.Shared
{
    [Serializable]
    public class DuplicateOrderException :
        Exception
    {
        public DuplicateOrderException()
        {
        }

        public DuplicateOrderException(string? message) : base(message)
        {
        }

        public DuplicateOrderException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
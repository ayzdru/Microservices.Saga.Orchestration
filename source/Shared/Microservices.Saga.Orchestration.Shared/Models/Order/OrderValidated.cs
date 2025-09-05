namespace Sample.Components.Consumers;

public record OrderValidated
{
    public Guid OrderId { get; init; }
}
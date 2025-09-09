namespace Orchestration.Core.Models.Order;

public record OrderValidated
{
    public Guid OrderId { get; init; }
}
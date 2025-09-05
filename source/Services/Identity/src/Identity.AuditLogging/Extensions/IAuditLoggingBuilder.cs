using Microsoft.Extensions.DependencyInjection;

namespace Identity.AuditLogging.Extensions
{
    public interface IAuditLoggingBuilder
    {
        IServiceCollection Services { get; }
    }
}
namespace Identity.AuditLogging.Events
{
    public interface IAuditAction
    {
        object Action { get; set; }
    }
}
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.Client;

public class ClientSecretDeletedEvent : AuditEvent
{
    public ClientSecretDeletedEvent(int clientId, int clientSecretId)
    {
        ClientId = clientId;
        ClientSecretId = clientSecretId;
    }

    public int ClientId { get; set; }

    public int ClientSecretId { get; set; }
}
using Identity.BusinessLogic.Dtos.Configuration;
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.Client;

public class ClientClaimsRequestedEvent : AuditEvent
{
    public ClientClaimsRequestedEvent(ClientClaimsDto clientClaims)
    {
        ClientClaims = clientClaims;
    }

    public ClientClaimsDto ClientClaims { get; set; }
}
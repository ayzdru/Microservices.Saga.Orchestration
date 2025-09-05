using Identity.BusinessLogic.Dtos.Configuration;
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.Client;

public class ClientClaimRequestedEvent : AuditEvent
{
    public ClientClaimRequestedEvent(ClientClaimsDto clientClaims)
    {
        ClientClaims = clientClaims;
    }

    public ClientClaimsDto ClientClaims { get; set; }
}
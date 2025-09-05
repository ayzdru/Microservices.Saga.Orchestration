using Identity.BusinessLogic.Dtos.Configuration;
using Identity.AuditLogging.Events;

namespace Identity.BusinessLogic.Events.Client;

public class ClientClaimDeletedEvent : AuditEvent
{
    public ClientClaimDeletedEvent(ClientClaimsDto clientClaim)
    {
        ClientClaim = clientClaim;
    }

    public ClientClaimsDto ClientClaim { get; set; }
}
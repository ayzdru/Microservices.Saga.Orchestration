using System.Collections.Generic;
using Identity.EntityFramework.Configuration.Configuration.Identity;

namespace Identity.EntityFramework.Configuration.Configuration.IdentityServer;

public class Client : global::IdentityServer4.Models.Client
{
    public List<Claim> ClientClaims { get; set; } = new();
}
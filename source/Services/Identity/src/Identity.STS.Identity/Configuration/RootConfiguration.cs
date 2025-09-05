using Identity.Shared.Configuration.Configuration.Identity;

namespace Identity.STS.Identity.Configuration;

public class RootConfiguration : IRootConfiguration
{
    public AdminConfiguration AdminConfiguration { get; } = new();
    public RegisterConfiguration RegisterConfiguration { get; } = new();
}
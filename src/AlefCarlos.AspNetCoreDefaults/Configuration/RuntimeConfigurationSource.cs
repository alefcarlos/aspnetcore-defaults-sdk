using Microsoft.Extensions.Configuration;

namespace AlefCarlos.AspNetCoreDefaults.Configuration;

internal sealed class RuntimeConfigurationSource : IConfigurationSource
{
    public RuntimeConfigurationProvider Provider { get; } = [];

    public IConfigurationProvider Build(IConfigurationBuilder builder)
        => Provider;
}
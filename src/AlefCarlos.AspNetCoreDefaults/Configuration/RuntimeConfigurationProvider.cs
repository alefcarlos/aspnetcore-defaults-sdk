using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.Logging;

namespace AlefCarlos.AspNetCoreDefaults.Configuration;

internal sealed class RuntimeConfigurationProvider : MemoryConfigurationProvider
{
    public RuntimeConfigurationProvider()
        : base(new MemoryConfigurationSource())
    {
    }

    public IReadOnlyDictionary<string, string?> GetAll()
    {
        return Data.ToDictionary();
    }

    public void SetValue(string key, string value)
    {
        Set(key, value);
        OnReload();
    }

    private void SetInternal(string key, string value)
    {
        Set(key, value);
    }

    public void SetLogLevel(LogLevel level)
    {
        SetInternal("Logging:LogLevel:Default", level.ToString());
        SetInternal("Logging:Console:LogLevel:Default", level.ToString());

        OnReload();
    }
}

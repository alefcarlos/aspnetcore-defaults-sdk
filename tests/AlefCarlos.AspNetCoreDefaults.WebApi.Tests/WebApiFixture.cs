using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace AlefCarlos.AspNetCoreDefaults.WebApi.Tests;

public class WebApiFixture : WebApplicationFactory<Program>
{
    private static readonly KeyValuePair<string, string?>[] _inMemorySettings =
    [
        new("WEBAPI_SUT", "true"),
    ];

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(builder =>
        {
            builder.AddInMemoryCollection(_inMemorySettings);
        });

        builder.UseEnvironment("Development");

        return base.CreateHost(builder);
    }
}

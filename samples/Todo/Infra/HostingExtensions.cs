using Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UseCases;

namespace Microsoft.Extensions.Hosting;

public static class HostingExtensions
{
    public static IHostApplicationBuilder AddInfra(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("postgresdb")));
        builder.EnrichNpgsqlDbContext<ApplicationDbContext>();

        builder.Services.AddHostedService<MigrationTask>();
        return builder;
    }
}

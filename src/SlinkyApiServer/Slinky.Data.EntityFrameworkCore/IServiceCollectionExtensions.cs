using Microsoft.Extensions.DependencyInjection;

namespace Slinky.Data.EntityFrameworkCore;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection UseDebugEFCoreDatabaseServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddDbContext<SlinkyContext, InMemorySlinkyContext>(ServiceLifetime.Scoped, ServiceLifetime.Scoped)
            .AddScoped<IDatabaseService, DatabaseService>();
    }

    public static IServiceCollection UseCachingDebugEFCoreDatabaseServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddDbContext<SlinkyContext, InMemorySlinkyContext>(ServiceLifetime.Scoped, ServiceLifetime.Scoped)
            .AddDistributedMemoryCache()
            .AddScoped<IDatabaseService, CachingDatabaseService>();
    }
}

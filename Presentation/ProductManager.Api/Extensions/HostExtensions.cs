using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProductManager.Api.Extensions;


public static class HostExtensions
{
    public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder,
            int? retry = 0) where TContext : IdentityDbContext
    {
        var retryForAvailability = retry!.Value;
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<TContext>>();
        var context = services.GetService<TContext>();
        try
        {
            logger.LogInformation("Migrating Started For Sql Server");
            InvokeSeeder(seeder!, context, services);
            logger.LogInformation("Migrating Has Been Done For Sql Server");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An Error Occured While Migrating Database");
            if (retryForAvailability < 3)
            {
                retryForAvailability++;
                Thread.Sleep(2000);
                MigrateDatabase(host, seeder, retryForAvailability);
            }
            throw;
        }
        return host;
    }

    private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context,
        IServiceProvider services) where TContext : IdentityDbContext?
    {
        context?.Database.Migrate();
        seeder(context!, services);
    }
}
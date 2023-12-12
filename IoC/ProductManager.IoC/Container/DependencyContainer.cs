using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductManager.Data.Context;
using ProductManager.Data.Repositories.Common;
using ProductManager.Domain.Repositories.Common;
using System.Reflection;


namespace ProductManager.IoC.Container;

public static class DependencyContainer
{
    public static IServiceCollection UseIoC(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        #region Database

        serviceCollection.AddDbContext<ProductManagerContext>(options => options.UseSqlServer(configuration.GetConnectionString("ProductManagerConnection")));

        #endregion

        #region MediatR

        serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
        serviceCollection.AddScoped<Mediator>();
        #endregion

        #region AutoMapper

        serviceCollection.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        #endregion

        #region Service Dependencies

        serviceCollection.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        #endregion

        #region Add Services

        serviceCollection.Scan(s =>
            s.FromAssemblies(Assembly.Load("ProductManager.Application"))
                .AddClasses(c => c.Where(e => e.Name.EndsWith("Service")))
                .AsImplementedInterfaces()
                .WithTransientLifetime());

        #endregion

        return serviceCollection;
    }
}
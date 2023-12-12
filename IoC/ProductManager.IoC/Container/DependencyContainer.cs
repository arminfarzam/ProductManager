using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductManager.Data.Context;
using System.Reflection;
using ProductManager.Application.Services.Implementations;
using ProductManager.Application.Services.Interfaces;


namespace ProductManager.IoC.Container;

public static class DependencyContainer
{
    public static IServiceCollection UseIoC(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        #region Database

        serviceCollection.AddDbContext<ProductManagerContext>(options => options.UseSqlServer(configuration.GetConnectionString("ProductManagerConnection")));

        #endregion

        #region MediatR

        //serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        //serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(AppDomain.CurrentDomain.GetAssemblies()));

        #endregion

        #region AutoMapper

        serviceCollection.AddAutoMapper(Assembly.GetExecutingAssembly());

        #endregion

        #region Service Dependencies

        serviceCollection.AddTransient<IAuthenticationService, AuthenticationService>();

        #endregion

        return serviceCollection;
    }
}
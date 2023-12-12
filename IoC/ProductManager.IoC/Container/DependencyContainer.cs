using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductManager.Data.Context;
using Microsoft.AspNetCore.Identity;


namespace ProductManager.IoC.Container;

public static class DependencyContainer
{
    public static IServiceCollection UseIoC(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        #region Database

        serviceCollection.AddDbContext<ProductManagerContext>(options => options.UseSqlServer(configuration.GetConnectionString("ProductManagerConnection")));

        #endregion

        return serviceCollection;
    }
}
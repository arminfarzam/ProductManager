using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ProductManager.Application.DTOs.Configuration;
using ProductManager.Application.DTOs.Identity;
using ProductManager.Application.Services.Implementations;
using ProductManager.Application.Services.Interfaces;
using ProductManager.Data.Context;

namespace ProductManager.Test;

public class AuthenticationTest
{
    [Fact]
    public async Task RegisterUserShouldReturnsTrue()
    {
        // Arrange
        var serviceProvider = GetServiceProvider();
        var authenticationService = serviceProvider.GetRequiredService<IAuthenticationService>();

        // Act
        var result = await authenticationService.RegisterUser(new RegisterUserDto { Username = "arminfrzm75", Password = "@1q2w3E*", Email = "email@example.com", PhoneNumber = "09215478965" });

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task LoginUserShouldReturnsAuthResponseDto()
    {
        // Arrange
        var serviceProvider = GetServiceProvider();
        var authenticationService = serviceProvider.GetRequiredService<IAuthenticationService>();
        await authenticationService.RegisterUser(new RegisterUserDto { Username = "arminfrzm75", Password = "@1q2w3E*", Email = "email@example.com", PhoneNumber = "09215478965" });

        // Act
        var result = await authenticationService.LoginUser(new LoginUserDto { Username = "arminfrzm75", Password = "@1q2w3E*" });

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.AccessToken);
        Assert.NotEqual(default(DateTime), result.Expiration);
        Assert.NotNull(result.UserId);
    }

    private static IServiceProvider GetServiceProvider()
    {
        var services = new ServiceCollection();
        var jwtConfiguration = new JwtConfigurationDto
        {
            Secret = "ThisIsTestJwtSecretThisIsTestJwtSecret",
            ValidIssuer = "",
            ValidAudience = "",
            TokenValidityInMinutes = 60
        };
        services.AddSingleton(Options.Create(jwtConfiguration));
        services.AddOptions();
        var dbContextOptions = new DbContextOptionsBuilder<ProductManagerContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        services.AddSingleton(new ProductManagerContext(dbContextOptions));
        services.AddIdentityCore<IdentityUser>()
            .AddEntityFrameworkStores<ProductManagerContext>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        return services.BuildServiceProvider();
    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductManager.Application.DTOs.Identity;
using ProductManager.Application.Services.Implementations;
using ProductManager.Application.Services.Interfaces;
using ProductManager.Data.Context;

namespace ProductManager.Test;

public class AuthenticationTest
{
    [Fact]
    public async Task RegisterUser_ValidUser_ReturnsTrue()
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

    [Fact]
    public async Task LoginUser_ShouldFailed()
    {
        // Arrange
        var serviceProvider = GetServiceProvider();
        var authenticationService = serviceProvider.GetRequiredService<IAuthenticationService>();

        // Register a user before attempting to log in
        await authenticationService.RegisterUser(new RegisterUserDto { Username = "arminfrzm75", Password = "@1q2w3E*", Email = "email@example.com", PhoneNumber = "09215478965" });

        // Act
        Action act = async () => await authenticationService.LoginUser(new LoginUserDto { Username = "arminfrzm75", Password = "@1q2w3W*" });

        // Assert
        Assert.ThrowsAny<Exception>(act);
    }
   
    [Fact]
    public async Task ShouldNotRegisterUserWithWrongPhoneNumber()
    {
        // Arrange
        var serviceProvider = GetServiceProvider();
        var authenticationService = serviceProvider.GetRequiredService<IAuthenticationService>();

        // Act
        var result= await authenticationService.RegisterUser(new RegisterUserDto { Username = "arminfrzm76", Password = "@1q2w3E*", Email = "email@example.com", PhoneNumber = "215478965" });

        // Assert
        Assert.False(result);
    }

    private static IServiceProvider GetServiceProvider()
    {
        var services = new ServiceCollection();
        var dbContextOptions = new DbContextOptionsBuilder<ProductManagerContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        services.AddSingleton(new ProductManagerContext(dbContextOptions));
        // Add Identity services
        services.AddIdentityCore<IdentityUser>()
            .AddEntityFrameworkStores<ProductManagerContext>();
        // Add your authentication service
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        return services.BuildServiceProvider();
    }
}
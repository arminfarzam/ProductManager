using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ProductManager.Data.Context;

public class ProductManagerContextSeed
{
    public static async Task SeedAsync(ProductManagerContext productManagerContext, ILogger<ProductManagerContextSeed> logger)
    {
        if (!await productManagerContext.Users.AnyAsync())
        {
            await productManagerContext.Users.AddRangeAsync(GetPreconfiguredUsers());
            await productManagerContext.SaveChangesAsync();
            logger.LogInformation("Data Seed Configured");
        }
    }

    public static IEnumerable<IdentityUser> GetPreconfiguredUsers()
    {
        var hasher = new PasswordHasher<IdentityUser>();
        return new List<IdentityUser>
        {
            new()
            {
               
                Id = "76949d09-f232-4b6f-bc4f-707f1931791d",
                UserName = "arminfrzm72",
                NormalizedUserName = "ARMINFRZM72",
                Email = "armin.frzm72@gmail.com",
                NormalizedEmail = "ARMIN.FRZM72@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null!, "@1q2w3E*"),
                SecurityStamp = Guid.NewGuid().ToString("D"),
                PhoneNumber = "09215483851",
                PhoneNumberConfirmed = true,
                LockoutEnabled = true
            }
        };
    }
}
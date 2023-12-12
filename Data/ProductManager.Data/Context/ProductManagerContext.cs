using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProductManager.Data.Context;

public class ProductManagerContext:IdentityDbContext
{
    public ProductManagerContext(DbContextOptions<ProductManagerContext> options):base(options)
    {
        
    }

}
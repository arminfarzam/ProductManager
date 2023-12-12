using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductManager.Domain.Entities.Product;

namespace ProductManager.Data.Context;

public class ProductManagerContext:IdentityDbContext
{
    public ProductManagerContext(DbContextOptions<ProductManagerContext> options):base(options)
    {
        
    }

    #region DbSets

    public DbSet<Product>? Products { get; set; }

    #endregion

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        #region Modify MetaDataIndex

        builder.Entity<Product>().HasIndex(p => new { p.ProduceDate, p.ManufacturerEmail }).HasDatabaseName("ProductIndex").IsUnique();

        #endregion

    }
}
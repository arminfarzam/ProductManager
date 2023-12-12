using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductManager.Domain.Entities.Common;
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

        builder.Entity<Product>().HasIndex(p => new { p.CreateDate, p.ManufacturerEmail }).HasDatabaseName("ProductIndex").IsUnique();

        #endregion

    }

    #region Overrides

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            entry.Entity.LastModifiedDate = DateOnly.FromDateTime(DateTime.Now);

            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreateDate = DateOnly.FromDateTime(DateTime.Now);
            }
        }
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    #endregion
}
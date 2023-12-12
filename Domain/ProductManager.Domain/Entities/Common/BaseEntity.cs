using System.ComponentModel.DataAnnotations;

namespace ProductManager.Domain.Entities.Common;

public class BaseEntity
{
    [Key]
    public Guid Id { get; set; }
    public DateOnly CreateDate { get; set; }
    public DateOnly LastModifiedDate { get; set; }
    public bool IsDeleted { get; set; }
}
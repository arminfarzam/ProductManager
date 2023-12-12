using System.ComponentModel.DataAnnotations;

namespace ProductManager.Domain.Entities.Common;

public class BaseEntity
{
    [Key]
    public Guid Id { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime LastModifiedDate { get; set; }
    public bool IsDeleted { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace ProductManager.Domain.Entities.Product;

public class Product
{
    public Guid ProductId { get; set; }

    [Required] 
    public string Name { get; set; } = string.Empty;

    public DateTime ProduceDate { get; set; }

    [Required]
    [EmailAddress]
    public string ManufacturerEmail { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^09[0|1|2|3][0-9]{8}$")]
    public string ManufacturerPhone { get; set;} = string.Empty;

    public bool IsAvailable { get; set; }

    [Required]
    public string CreatorId { get; set; } = string.Empty;
}

using System.ComponentModel.DataAnnotations;
using ProductManager.Domain.Entities.Common;

namespace ProductManager.Domain.Entities.Product;

public class Product:BaseEntity
{

    [Required] 
    public string Name { get; set; } = null!;

    [Required] 
    [EmailAddress] 
    public string ManufacturerEmail { get; set; } = null!;

    [Required]
    [RegularExpression("^09[0|1|2|3][0-9]{8}$")]
    public string ManufacturerPhone { get; set;} = null!;

    public bool IsAvailable { get; set; }

    [Required]
    public string CreatorUserName { get; set; } = null!;
}

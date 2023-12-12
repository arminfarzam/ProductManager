using System.ComponentModel.DataAnnotations;

namespace ProductManager.Application.DTOs.Product;

public class AddOrEditProductDto
{
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string ManufacturerEmail { get; set; } = null!;

    [Required]
    public string ManufacturerPhone { get; set; } = null!;

    public bool IsAvailable { get; set; }
}
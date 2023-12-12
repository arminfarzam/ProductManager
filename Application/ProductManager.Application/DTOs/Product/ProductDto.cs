namespace ProductManager.Application.DTOs.Product;

public class ProductDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateTime ProduceDate { get; set; }

    public string ManufacturerEmail { get; set; } = string.Empty;

    public string ManufacturerPhone { get; set; } = string.Empty;

    public bool IsAvailable { get; set; }

    public string CreatorName { get; set; } = string.Empty;
}

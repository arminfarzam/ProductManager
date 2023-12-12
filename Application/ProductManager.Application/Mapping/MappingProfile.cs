using AutoMapper;
using ProductManager.Application.DTOs.Product;
using ProductManager.Domain.Entities.Product;

namespace ProductManager.Application.Mapping;

public class MappingProfile:Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<AddOrEditProductDto, Product>().ReverseMap();


    }
}
using AutoMapper;
using ProductManager.Application.DTOs.Product;
using ProductManager.Domain.Entities.Product;

namespace ProductManager.Application.Mapping;

public class MappingProfile:Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>().ForMember(p=>p.ProduceDate,i=>i.MapFrom(p=>p.CreateDate))
            .ForMember(p => p.CreatorName, i => i.MapFrom(p => p.CreatorUserName));
        CreateMap<AddOrEditProductDto, Product>().ReverseMap();


    }
}
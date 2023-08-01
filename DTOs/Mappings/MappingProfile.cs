using APICatalog.Models;
using AutoMapper;

namespace APICatalog.DTOs.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ProductModel, ProductInputDTO>().ReverseMap();
        CreateMap<ProductModel, ProductOutputDTO>().ReverseMap();
        CreateMap<CategoryModel, CategoryInputDTO>().ReverseMap();
        CreateMap<CategoryModel, CategoryOutputDTO>().ReverseMap();
    }
}

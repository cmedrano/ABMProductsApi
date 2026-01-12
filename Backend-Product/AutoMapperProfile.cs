using AutoMapper;
using Products.Application.Dtos;
using Products.Domain.Entities;

namespace Backend_Product
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            // Mapeo de Client a ClientDto
            CreateMap<Client, ClientDto>();
            CreateMap<ClientDto, Client>();
            CreateMap<CreateClientDto, Client>();
            CreateMap<UpdateClientDto, Client>();

            // Mapeo de Product a ProductDto
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.ProductCategories.Select(pc => pc.Category)));

            // Mapeo de ProductDto a Product
            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.ProductCategories, opt => opt.Ignore());

            // Mapeo de CreateProductDto a Product
            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.ProductCategories, opt => opt.Ignore());

            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.ProductCategories, opt => opt.Ignore());

            CreateMap<Category, CategoryDto>();// Mapeo de Category a CategoryDto           
            CreateMap<CategoryDto, Category>();// Mapeo de CategoryDto a Category

        }
    }
}

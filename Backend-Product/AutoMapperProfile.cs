using AutoMapper;
using Products.Application.Dtos;
using Products.Domain.Entities;

namespace Backend_Product
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            // Mapeo de Product a ProductDto
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.ProductCategories.Select(pc => pc.Category)));

            // Mapeo de ProductDto a Product
            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.ProductCategories, opt => opt.Ignore());

            // Mapeo de CreateProductDto a Product
            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.ProductCategories, opt => opt.Ignore());

            CreateMap<Category, CategoryDto>();// Mapeo de Category a CategoryDto           
            CreateMap<CategoryDto, Category>();// Mapeo de CategoryDto a Category

            // Mapeo para cuando Category tiene Products
            //CreateMap<Category, CategoryWithProductsDto>()
            //    .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.ProductCategories.Select(pc => pc.Product)));
        }
    }
}

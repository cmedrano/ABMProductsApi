using Products.Application.Dtos;

namespace Products.Application.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProducts();
        Task<ProductDto> GetProductById(int id);
        Task<ProductDto> CreateProduct(CreateProductDto productDto);
        Task<ProductDto> UpdateProduct(UpdateProductDto productDto);
        Task<bool> DeleteProduct(int id);
        Task<IEnumerable<ProductDto>> FilterProducts(int categoryId, string productName);
    }
}

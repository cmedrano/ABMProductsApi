using Products.Domain.Entities;

namespace Products.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
        Task<IEnumerable<Product>> FilterProductsAsync(int categoryId, string productName);
    }
}

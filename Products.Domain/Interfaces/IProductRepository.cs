using Products.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> getAllProductAsync();
        Task<Product> getProductByIdAsync(int id);
        Task addProductAsync(Product product);
        Task updateProductAsync(Product product);
        Task deleteProductAsync(int id);
        Task<IEnumerable<Product>> FilterProductsAsync(int categoryId, string productName);
    }
}

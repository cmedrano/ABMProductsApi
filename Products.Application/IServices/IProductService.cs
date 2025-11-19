using Products.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Application.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> getAllProducts();
        Task<ProductDto> getProductById(int id);
        Task<ProductDto> createProduct(CreateProductDto productDto);
        Task<ProductDto> updateProduct(UpdateProductDto productDto);
        Task<bool> deleteProduct(int id);
        Task<IEnumerable<ProductDto>> FilterProducts(int categoryId, string productName);
    }
}

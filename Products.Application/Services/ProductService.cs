using AutoMapper;
using Products.Application.Dtos;
using Products.Application.IServices;
using Products.Domain.Entities;
using Products.Domain.Interfaces;

namespace Products.Application.Services
{
    public class ProductService : IProductService
    {
        readonly IProductRepository _productRepository;
        readonly ICategoryRepository _categoryRepository;
        readonly IMapper _mapper;
        public ProductService(IMapper mapper, IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProducts()
        {
            var products = await _productRepository.GetAllProductAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            return _mapper.Map<ProductDto>(product);
        }
        public async Task<ProductDto> CreateProduct(CreateProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);

            // Manejar categorías
            foreach (var categoryId in productDto.Categories)
            {
                var existingCategory = await _categoryRepository.GetCategoryByIdAsync(categoryId);
                if (existingCategory == null)
                    throw new ArgumentException($"The category with ID {categoryId} does not exist.");

                product.ProductCategories.Add(new ProductCategory
                {
                    CategoryId = categoryId
                });
            }

            await _productRepository.AddProductAsync(product);

            var createdProduct = await _productRepository.GetProductByIdAsync(product.Id);
            return _mapper.Map<ProductDto>(createdProduct);

        }

        public async Task<ProductDto> UpdateProduct(UpdateProductDto productDto)
        {
            var existingProduct = await _productRepository.GetProductByIdAsync(productDto.Id);
            if (existingProduct == null)
                throw new Exception($"Product with ID:{productDto.Id} not found");

            _mapper.Map(productDto, existingProduct);

            existingProduct.ProductCategories.Clear();
            foreach (var categoryId in productDto.Categories)
            {
                var existingCategory = await _categoryRepository.GetCategoryByIdAsync(categoryId);
                if (existingCategory == null)
                    throw new ArgumentException($"The category with ID {categoryId} does not exist.");

                existingProduct.ProductCategories.Add(new ProductCategory
                {
                    ProductId = existingProduct.Id,
                    CategoryId = categoryId,
                });
            }

            await _productRepository.UpdateProductAsync(existingProduct);

            // Recargar el producto con las categorías
            var updatedProduct = await _productRepository.GetProductByIdAsync(existingProduct.Id);
            return _mapper.Map<ProductDto>(updatedProduct);
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var existingProduct = await _productRepository.GetProductByIdAsync(id);
            if (existingProduct == null)
                return false;

            await _productRepository.DeleteProductAsync(id);
            return true;
        }

        public async Task<IEnumerable<ProductDto>> FilterProducts(int categoryId, string productName)
        {
            var products = await _productRepository.FilterProductsAsync(categoryId, productName);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }
    }
}

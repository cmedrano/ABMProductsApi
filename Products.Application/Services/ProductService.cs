using AutoMapper;
using Products.Application.Dtos;
using Products.Application.IServices;
using Products.Domain.Entities;
using Products.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<ProductDto>> getAllProducts()
        {
            var products = await _productRepository.getAllProductAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> getProductById(int id)
        {
            var product = await _productRepository.getProductByIdAsync(id);
            return _mapper.Map<ProductDto>(product);
        }
        public async Task<ProductDto> createProduct(CreateProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);

            // Manejar categorías
            foreach (var categoryId in productDto.Categories)
            {
                var existingCategory = await _categoryRepository.getCategoryByIdAsync(categoryId);
                if (existingCategory == null)
                    throw new ArgumentException($"The category with ID {categoryId} does not exist.");

                product.ProductCategories.Add(new ProductCategory
                {
                    CategoryId = categoryId
                });
            }

            await _productRepository.addProductAsync(product);

            var createdProduct = await _productRepository.getProductByIdAsync(product.Id);
            return _mapper.Map<ProductDto>(createdProduct);

        }

        public async Task<ProductDto> updateProduct(UpdateProductDto productDto)
        {
            var existingProduct = await _productRepository.getProductByIdAsync(productDto.Id);
            if (existingProduct == null)
                throw new Exception($"Product with ID:{productDto.Id} not found");

            _mapper.Map(productDto, existingProduct);

            existingProduct.ProductCategories.Clear();
            foreach (var categoryId in productDto.Categories)
            {
                var existingCategory = await _categoryRepository.getCategoryByIdAsync(categoryId);
                if (existingCategory == null)
                    throw new ArgumentException($"The category with ID {categoryId} does not exist.");

                existingProduct.ProductCategories.Add(new ProductCategory
                {
                    ProductId = existingProduct.Id,
                    CategoryId = categoryId,
                });
            }

            await _productRepository.updateProductAsync(existingProduct);

            // Recargar el producto con las categorías
            var updatedProduct = await _productRepository.getProductByIdAsync(existingProduct.Id);
            return _mapper.Map<ProductDto>(updatedProduct);
        }

        public async Task<bool> deleteProduct(int id)
        {
            var existingProduct = await _productRepository.getProductByIdAsync(id);
            if (existingProduct == null)
                return false;

            await _productRepository.deleteProductAsync(id);
            return true;
        }

        public async Task<IEnumerable<ProductDto>> FilterProducts(int categoryId, string productName)
        {
            var products = await _productRepository.FilterProductsAsync(categoryId, productName);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }
    }
}

using AutoMapper;
using Products.Application.Dtos;
using Products.Domain.Entities;
using Products.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Application.Services
{
    public class ProductService
    {
        readonly IProductRepository _repository;
        readonly IMapper _mapper;
        public ProductService(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> getAllProducts()
        {
            var products = await _repository.getAllProductAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task<ProductDto> getProductById(int id)
        {
            if (id <= 0)
                throw new Exception($"El Id {id} es invalido.");

            var product = await _repository.getProductByIdAsync(id);
            return _mapper.Map<ProductDto>(product);
        }
        public async Task<ProductDto> createProduct(CreateProductDto productDto)
        {
            if (productDto == null)
                throw new Exception($"Todos los Campos son Obligatorio");

            var product = _mapper.Map<Product>(productDto);

            // Manejar las categorías
            foreach (var categoryId in productDto.Categories)
            {
                product.ProductCategories.Add(new ProductCategory
                {
                    CategoryId = categoryId.Id,
                });
            }

            await _repository.addProductAsync(product);
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> updateProduct(ProductDto productDto)
        {
            var existingProduct = await _repository.getProductByIdAsync(productDto.Id);
            if (existingProduct == null)
                throw new Exception($"Producto con Id {productDto.Id} no encontrado.");

            _mapper.Map(productDto, existingProduct);

            // Actualizar categorías si es necesario
            if (productDto.Categories != null && productDto.Categories.Any())
            {
                existingProduct.ProductCategories.Clear();
                foreach (var categoryId in productDto.Categories)
                {
                    existingProduct.ProductCategories.Add(new ProductCategory
                    {
                        ProductId = existingProduct.Id,
                        CategoryId = categoryId.Id,
                    });
                }
            }

            await _repository.updateProductAsync(existingProduct);
            return _mapper.Map<ProductDto>(existingProduct);
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var existingProduct = await _repository.getProductByIdAsync(id);
            if (existingProduct == null)
                return false;

            await _repository.deleteProductAsync(id);
            return true;
        }

        public async Task<IEnumerable<ProductDto>> filterProductsByCategory(int categoryId)
        {
            var products = await _repository.filterProductsByCategoryAsync(categoryId);
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }



    }
}

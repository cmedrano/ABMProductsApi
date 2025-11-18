using Microsoft.EntityFrameworkCore;
using Products.Application.Dtos;
using Products.Domain.Entities;
using Products.Domain.Interfaces;
using Products.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
       readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> getAllProductAsync()
        {
            var result = await _context.Products
                            .Include(p => p.ProductCategories)
                            .ThenInclude(pc => pc.Category)
                            .ToListAsync();
            return result;
        }

        public async Task<Product> getProductByIdAsync(int id)
        {
            var result = await _context.Products
                                .Include(p => p.ProductCategories)
                                .ThenInclude(pc => pc.Category)
                                .FirstOrDefaultAsync(p => p.Id == id);
            return result;
        }

        public async Task addProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task updateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task deleteProductAsync(int id)
        {
            var existingProduct = await getProductByIdAsync(id);
            if (existingProduct != null)
            {
                _context.Products.Remove(existingProduct);
                await _context.SaveChangesAsync();
            }
        }

        // Filtro por categoria
        public async Task<IEnumerable<Product>> filterProductsByCategoryAsync(int categoryId)
        {
            var result = await _context.Products
                                .Include(p => p.ProductCategories)
                                .ThenInclude(pc => pc.Category)
                                .Where(p => p.ProductCategories.Any(pc => pc.CategoryId == categoryId))
                                .ToListAsync();
            return result;
        }


    }
}

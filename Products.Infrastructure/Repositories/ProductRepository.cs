using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;
using Products.Domain.Interfaces;
using Products.Infrastructure.Data;

namespace Products.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
       readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductAsync()
        {
            var result = await _context.Products
                            .Include(p => p.ProductCategories)
                            .ThenInclude(pc => pc.Category)
                            .ToListAsync();
            return result;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var result = await _context.Products
                                .Include(p => p.ProductCategories)
                                .ThenInclude(pc => pc.Category)
                                .FirstOrDefaultAsync(p => p.Id == id);
            return result;
        }

        public async Task AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var existingProduct = await GetProductByIdAsync(id);
            if (existingProduct != null)
            {
                _context.Products.Remove(existingProduct);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Product>> FilterProductsAsync(int categoryId, string productName)
        {
            var query = _context.Products
                        .Include(p => p.ProductCategories)
                        .ThenInclude(pc => pc.Category)
                        .AsQueryable();

            if (categoryId > 0)
            {
                query = query.Where(p => p.ProductCategories.Any(pc => pc.CategoryId == categoryId));
            }

            if (!string.IsNullOrWhiteSpace(productName))
            {
                query = query.Where(p => p.Name.ToLower().Contains(productName.ToLower()));
            }

            return await query.ToListAsync();
        }
    }
}

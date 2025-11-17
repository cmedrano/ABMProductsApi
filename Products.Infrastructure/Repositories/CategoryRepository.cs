using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;
using Products.Domain.Interfaces;
using Products.Infrastructure.Context;

namespace Products.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ProductsDbContext _context;

        public CategoryRepository(ProductsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

    }
}

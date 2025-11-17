using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;

namespace Products.Infrastructure.Context
{
    public class ProductsDbContext : DbContext
    {
        public ProductsDbContext(DbContextOptions<ProductsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories => Set<Category>();
    }
}

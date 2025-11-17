using Products.Domain.Entities;

namespace Products.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
    }
}

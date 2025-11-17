using Products.Application.Dtos;
using Products.Domain.Interfaces;

namespace Products.Application.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _repo;

        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<CategoryDto>> GetAll()
        {
            var categories = await _repo.GetAllAsync();

            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            });
        }
    }
}

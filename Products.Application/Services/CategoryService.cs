using AutoMapper;
using Products.Application.Dtos;
using Products.Application.IServices;
using Products.Domain.Interfaces;

namespace Products.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }
    }
}

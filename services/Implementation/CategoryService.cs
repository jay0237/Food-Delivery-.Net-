using FoodOrderingSystem.Models.Entities;
using FoodOrderingSystem.Repositories.Interfaces;
using FoodOrderingSystem.Services.Interfaces;

namespace FoodOrderingSystem.Services.Implementation;

public class CategoryService : ICategoryService
{
	private readonly ICategoryRepository _categoryRepository;

	public CategoryService(ICategoryRepository categoryRepository)
	{
		_categoryRepository = categoryRepository;
	}

	public async Task<IEnumerable<Category>> GetAllAsync()
	{
		return await _categoryRepository.GetAllAsync();
	}
}

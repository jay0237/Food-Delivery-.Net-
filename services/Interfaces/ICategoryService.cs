using FoodOrderingSystem.Models.Entities;

namespace FoodOrderingSystem.Services.Interfaces;

public interface ICategoryService
{
	Task<IEnumerable<Category>> GetAllAsync();
}

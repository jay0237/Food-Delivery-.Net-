using FoodOrderingSystem.Models.Entities;

namespace FoodOrderingSystem.Repositories.Interfaces;

public interface IFoodRepository
{
    Task<IEnumerable<Food>> GetAllAsync();

    Task<Food?> GetByIdAsync(int id);

    Task<bool> ExistsAsync(string name, string restaurantName);

    Task AddAsync(Food food);

    Task UpdateAsync(Food food);

    Task DeleteAsync(int id);
}

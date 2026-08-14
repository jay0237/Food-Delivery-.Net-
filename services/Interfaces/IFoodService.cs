using FoodOrderingSystem.Models.Entities;

namespace FoodOrderingSystem.Services.Interfaces;

public interface IFoodService
{
    Task<IEnumerable<Food>> GetAllAsync();

    Task<Food?> GetByIdAsync(int id);

    Task AddAsync(Food food);

    Task UpdateAsync(Food food);

    Task DeleteAsync(int id);
}
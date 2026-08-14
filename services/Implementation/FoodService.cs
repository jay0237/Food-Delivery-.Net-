using FoodOrderingSystem.Models.Entities;
using FoodOrderingSystem.Repositories.Interfaces;
using FoodOrderingSystem.Services.Interfaces;

namespace FoodOrderingSystem.Services.Implementations;

public class FoodService : IFoodService
{
    private readonly IFoodRepository _repository;

    public FoodService(IFoodRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Food>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Food?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task AddAsync(Food food)
    {
        await _repository.AddAsync(food);
    }

    public async Task UpdateAsync(Food food)
    {
        await _repository.UpdateAsync(food);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}
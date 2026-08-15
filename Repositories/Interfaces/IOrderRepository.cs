using FoodOrderingSystem.Models.Entities;

namespace FoodOrderingSystem.Repositories.Interfaces;

public interface IOrderRepository
{
    Task AddAsync(Order order);

    Task<Order?> GetByIdAsync(int id);

    Task<IEnumerable<Order>> GetByUserIdAsync(int userId);

    Task<IEnumerable<Order>> GetAllAsync();

    Task SaveChangesAsync();
}
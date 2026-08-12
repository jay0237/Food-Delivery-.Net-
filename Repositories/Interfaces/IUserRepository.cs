using FoodOrderingSystem.Models.Entities;

namespace FoodOrderingSystem.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);

    Task AddAsync(User user);
}
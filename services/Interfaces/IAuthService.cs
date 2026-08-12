using FoodOrderingSystem.Models.Entities;

namespace FoodOrderingSystem.Services.Interfaces;

public interface IAuthService
{
    Task<bool> RegisterAsync(
        string fullName,
        string email,
        string password);

    Task<User?> LoginAsync(
        string email,
        string password);
}
using FoodOrderingSystem.Models.Entities;

namespace FoodOrderingSystem.Services.Interfaces;

public interface ICartService
{
    Task<Cart> GetOrCreateCartAsync(int userId);

    Task<bool> AddToCartAsync(
        int userId,
        int foodId,
        int quantity);

    Task<bool> UpdateQuantityAsync(
        int userId,
        int foodId,
        int quantity);

    Task<bool> RemoveFromCartAsync(
        int userId,
        int foodId);
}
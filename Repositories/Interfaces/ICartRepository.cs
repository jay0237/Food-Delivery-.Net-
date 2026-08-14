using FoodOrderingSystem.Models.Entities;

namespace FoodOrderingSystem.Repositories.Interfaces;

public interface ICartRepository
{
    Task<Cart?> GetCartByUserIdAsync(int userId);

    Task<CartItem?> GetCartItemAsync(int cartId, int foodId);

    Task AddCartAsync(Cart cart);

    Task AddCartItemAsync(CartItem cartItem);

    Task UpdateCartItemAsync(CartItem cartItem);

    Task RemoveCartItemAsync(CartItem cartItem);

    Task SaveChangesAsync();
}
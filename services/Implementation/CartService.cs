using FoodOrderingSystem.Data;
using FoodOrderingSystem.Models.Entities;
using FoodOrderingSystem.Repositories.Interfaces;
using FoodOrderingSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingSystem.Services.Implementations;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly AppDbContext _context;

    public CartService(
        ICartRepository cartRepository,
        AppDbContext context)
    {
        _cartRepository = cartRepository;
        _context = context;
    }

    public async Task<Cart> GetOrCreateCartAsync(int userId)
    {
        var cart =
            await _cartRepository.GetCartByUserIdAsync(userId);

        if (cart != null)
        {
            return cart;
        }

        cart = new Cart
        {
            UserId = userId
        };

        await _cartRepository.AddCartAsync(cart);
        await _cartRepository.SaveChangesAsync();

        return cart;
    }

    public async Task<bool> AddToCartAsync(
        int userId,
        int foodId,
        int quantity)
    {
        if (quantity <= 0)
        {
            return false;
        }

        var food = await _context.Foods
            .FindAsync(foodId);

        if (food == null || !food.IsAvailable)
        {
            return false;
        }

        var cart =
            await GetOrCreateCartAsync(userId);

        var existingItem =
            await _cartRepository.GetCartItemAsync(
                cart.Id,
                foodId);

        if (existingItem != null)
        {
            existingItem.Quantity += quantity;

            await _cartRepository.UpdateCartItemAsync(
                existingItem);
        }
        else
        {
            var cartItem = new CartItem
            {
                CartId = cart.Id,
                FoodId = foodId,
                Quantity = quantity
            };

            await _cartRepository.AddCartItemAsync(
                cartItem);
        }

        await _cartRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateQuantityAsync(
        int userId,
        int foodId,
        int quantity)
    {
        var cart =
            await _cartRepository.GetCartByUserIdAsync(userId);

        if (cart == null)
        {
            return false;
        }

        var item =
            await _cartRepository.GetCartItemAsync(
                cart.Id,
                foodId);

        if (item == null)
        {
            return false;
        }

        if (quantity <= 0)
        {
            await _cartRepository.RemoveCartItemAsync(item);
        }
        else
        {
            item.Quantity = quantity;

            await _cartRepository.UpdateCartItemAsync(item);
        }

        await _cartRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> RemoveFromCartAsync(
        int userId,
        int foodId)
    {
        var cart =
            await _cartRepository.GetCartByUserIdAsync(userId);

        if (cart == null)
        {
            return false;
        }

        var item =
            await _cartRepository.GetCartItemAsync(
                cart.Id,
                foodId);

        if (item == null)
        {
            return false;
        }

        await _cartRepository.RemoveCartItemAsync(item);
        await _cartRepository.SaveChangesAsync();

        return true;
    }
}
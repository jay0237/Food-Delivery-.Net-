using FoodOrderingSystem.Data;
using FoodOrderingSystem.Models.Entities;
using FoodOrderingSystem.Repositories.Interfaces;
using FoodOrderingSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingSystem.Services.Implementations;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly AppDbContext _context;

    public OrderService(
        IOrderRepository orderRepository,
        ICartRepository cartRepository,
        AppDbContext context)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _context = context;
    }

    public async Task<Order?> CreateOrderAsync(int userId)
    {
        var cart = await _cartRepository
            .GetCartByUserIdAsync(userId);

        if (cart == null || !cart.Items.Any())
        {
            return null;
        }

        var order = new Order
        {
            UserId = userId,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        decimal total = 0;

        foreach (var cartItem in cart.Items)
        {
            if (!cartItem.Food.IsAvailable)
            {
                continue;
            }

            var itemTotal =
                cartItem.Food.Price * cartItem.Quantity;

            total += itemTotal;

            order.Items.Add(new OrderItem
            {
                FoodId = cartItem.FoodId,
                Quantity = cartItem.Quantity,
                Price = cartItem.Food.Price
            });
        }

        if (!order.Items.Any())
        {
            return null;
        }

        order.TotalAmount = total;

        await _orderRepository.AddAsync(order);

        _context.CartItems.RemoveRange(cart.Items);

        await _orderRepository.SaveChangesAsync();

        return order;
    }

    public async Task<Order?> GetByIdAsync(int orderId)
    {
        return await _orderRepository.GetByIdAsync(orderId);
    }

    public async Task<IEnumerable<Order>> GetMyOrdersAsync(int userId)
    {
        return await _orderRepository.GetByUserIdAsync(userId);
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _orderRepository.GetAllAsync();
    }

    public async Task<bool> UpdateStatusAsync(
        int orderId,
        string status)
    {
        var order = await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
        {
            return false;
        }

        var allowedStatuses = new[]
        {
            "Pending",
            "Confirmed",
            "Preparing",
            "OutForDelivery",
            "Delivered",
            "Cancelled"
        };

        if (!allowedStatuses.Contains(status))
        {
            return false;
        }

        order.Status = status;

        await _context.SaveChangesAsync();

        return true;
    }
}
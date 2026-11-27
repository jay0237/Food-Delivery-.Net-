using FoodOrderingSystem.Models.Entities;

namespace FoodOrderingSystem.Services.Interfaces;

public interface IOrderService
{
    Task<Order?> CreateOrderAsync(
        int userId,
        string deliveryAddress,
        string? deliveryCity,
        string? deliveryState,
        string? deliveryZipCode,
        string deliveryPhone,
        string? deliveryInstructions,
        double? latitude,
        double? longitude);

    Task<Order?> GetByIdAsync(int orderId);

    Task<IEnumerable<Order>> GetMyOrdersAsync(int userId);

    Task<IEnumerable<Order>> GetAllOrdersAsync();

    Task<bool> UpdateStatusAsync(int orderId, string status);
}
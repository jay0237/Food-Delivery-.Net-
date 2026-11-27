namespace FoodOrderingSystem.Models.Entities;

public class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = "Pending";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string DeliveryAddress { get; set; } = string.Empty;

    public string? DeliveryCity { get; set; }

    public string? DeliveryState { get; set; }

    public string? DeliveryZipCode { get; set; }

    public string? DeliveryPhone { get; set; }

    public string? DeliveryInstructions { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public ICollection<OrderItem> Items { get; set; }
        = new List<OrderItem>();
}
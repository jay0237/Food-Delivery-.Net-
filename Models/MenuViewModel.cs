using FoodOrderingSystem.Models.Entities;

namespace FoodOrderingSystem.Models;

public class MenuViewModel
{
    public IReadOnlyList<Food> Foods { get; init; } = [];

    public IReadOnlyList<Category> Categories { get; init; } = [];

    public string? Search { get; init; }

    public int? CategoryId { get; init; }

    public decimal? MinPrice { get; init; }

    public decimal? MaxPrice { get; init; }

    public bool AvailableOnly { get; init; } = true;
}
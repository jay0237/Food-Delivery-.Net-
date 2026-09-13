using FoodOrderingSystem.Models.Entities;

namespace FoodOrderingSystem.Models;

public class HomeViewModel
{
    public IReadOnlyList<Category> Categories { get; init; } = [];

    public IReadOnlyList<Food> FeaturedFoods { get; init; } = [];

    public int MenuCount { get; init; }
}
namespace FoodOrderingSystem.Models.DTOs.OpenMenu;

public class OpenMenuItemDto
{
    public string MenuItemName { get; set; } = string.Empty;

    public string MenuItemDescription { get; set; } = string.Empty;

    public decimal? MenuItemPrice { get; set; }

    public string? ImageUrl { get; set; }

    public string RestaurantName { get; set; } = string.Empty;

    public string? CuisineTypePrimary { get; set; }

    public string? CityTown { get; set; }

    public string? StateProvince { get; set; }

    public string? Country { get; set; }

    public string? Address1 { get; set; }
}
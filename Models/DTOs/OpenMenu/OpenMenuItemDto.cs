using System.Text.Json.Serialization;

namespace FoodOrderingSystem.Models.DTOs.OpenMenu;

public class OpenMenuItemDto
{
    [JsonPropertyName("menu_item_name")]
    public string MenuItemName { get; set; } = string.Empty;

    [JsonPropertyName("menu_item_description")]
    public string MenuItemDescription { get; set; } = string.Empty;

    [JsonPropertyName("menu_item_price")]
    public decimal? MenuItemPrice { get; set; }

    [JsonPropertyName("image_url")]
    public string? ImageUrl { get; set; }

    [JsonPropertyName("restaurant_name")]
    public string RestaurantName { get; set; } = string.Empty;

    [JsonPropertyName("cuisine_type_primary")]
    public string? CuisineTypePrimary { get; set; }

    [JsonPropertyName("city_town")]
    public string? CityTown { get; set; }

    [JsonPropertyName("state_province")]
    public string? StateProvince { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("address1")]
    public string? Address1 { get; set; }
}
using System.Text.Json.Serialization;

namespace FoodOrderingSystem.Models.DTOs.OpenMenu;

public class OpenMenuSearchResponse
{
    [JsonPropertyName("response")]
    public OpenMenuResponse Response { get; set; } = new();
}

public class OpenMenuResponse
{
    [JsonPropertyName("api")]
    public OpenMenuApiInfo Api { get; set; } = new();

    [JsonPropertyName("result")]
    public OpenMenuResult Result { get; set; } = new();
}

public class OpenMenuApiInfo
{
    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("api_version")]
    public string? ApiVersion { get; set; }
}

public class OpenMenuResult
{
    [JsonPropertyName("restaurants")]
    public List<OpenMenuRestaurantDto> Restaurants { get; set; } = new();

    [JsonPropertyName("items")]
    public List<OpenMenuItemDto> Items { get; set; } = new();

    [JsonPropertyName("menus")]
    public List<OpenMenuDto> Menus { get; set; } = new();
}

public class OpenMenuDto
{
    [JsonPropertyName("restaurant_name")]
    public string RestaurantName { get; set; } = string.Empty;

    [JsonPropertyName("items")]
    public List<OpenMenuItemDto> Items { get; set; } = new();
}
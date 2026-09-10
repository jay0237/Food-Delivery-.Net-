using System.Text.Json.Serialization;

namespace FoodOrderingSystem.Models.DTOs.OpenMenu;

public class OpenMenuSearchResponse
{
    [JsonPropertyName("response")]
    public OpenMenuResponse? Response { get; set; }
}

public class OpenMenuResponse
{
    [JsonPropertyName("api")]
    public OpenMenuApiInfo? Api { get; set; }

    [JsonPropertyName("result")]
    public OpenMenuResult? Result { get; set; }
}

public class OpenMenuApiInfo
{
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("api_version")]
    public string? ApiVersion { get; set; }
}

public class OpenMenuResult
{
    [JsonPropertyName("restaurants")]
    public List<OpenMenuRestaurantDto>? Restaurants { get; set; }

    [JsonPropertyName("items")]
    public List<OpenMenuItemDto>? Items { get; set; }

    [JsonPropertyName("menus")]
    public List<OpenMenuDto>? Menus { get; set; }
}

public class OpenMenuDto
{
    [JsonPropertyName("restaurant_name")]
    public string RestaurantName { get; set; } = string.Empty;

    [JsonPropertyName("items")]
    public List<OpenMenuItemDto>? Items { get; set; }
}
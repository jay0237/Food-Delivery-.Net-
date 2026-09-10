using System.Text.Json.Serialization;

namespace FoodOrderingSystem.Models.DTOs.OpenMenu;

public class OpenMenuRestaurantDto
{
    [JsonPropertyName("restaurant_name")]
    public string RestaurantName { get; set; } = string.Empty;

    [JsonPropertyName("brief_description")]
    public string? BriefDescription { get; set; }

    [JsonPropertyName("address1")]
    public string? Address1 { get; set; }

    [JsonPropertyName("city_town")]
    public string? CityTown { get; set; }

    [JsonPropertyName("state_province")]
    public string? StateProvince { get; set; }

    [JsonPropertyName("postal_code")]
    public string? PostalCode { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("cuisine_type_primary")]
    public string? CuisineTypePrimary { get; set; }

    [JsonPropertyName("website_url")]
    public string? WebsiteUrl { get; set; }
}
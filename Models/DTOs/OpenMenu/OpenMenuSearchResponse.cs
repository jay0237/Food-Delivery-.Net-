using System.Text.Json.Serialization;

namespace FoodOrderingSystem.Models.DTOs.OpenMenu
{
    public class OpenMenuSearchResponse
    {
        [JsonPropertyName("response")]
        public OpenMenuResponseWrapper? Response { get; set; }
    }

    public class OpenMenuResponseWrapper
    {
        [JsonPropertyName("result")]
        public OpenMenuResult? Result { get; set; }
    }

    public class OpenMenuResult
    {
        [JsonPropertyName("items")]
        public List<OpenMenuItemResponse>? Items { get; set; }
    }

    public class OpenMenuItemResponse
    {
        [JsonPropertyName("menu_item_name")]
        public string? MenuItemName { get; set; }

        [JsonPropertyName("menu_item_description")]
        public string? MenuItemDescription { get; set; }

        [JsonPropertyName("menu_item_price")]
        public string? MenuItemPrice { get; set; } 

        [JsonPropertyName("image_url")]
        public string? ImageUrl { get; set; }

        [JsonPropertyName("restaurant_name")]
        public string? RestaurantName { get; set; }

        [JsonPropertyName("cuisine_type_primary")]
        public string? CuisineTypePrimary { get; set; }

        [JsonPropertyName("city_town")]
        public string? CityTown { get; set; }

        [JsonPropertyName("state_province")]
        public string? StateProvince { get; set; }

        [JsonPropertyName("country")]
        public string? Country { get; set; }

        [JsonPropertyName("address_1")]
        public string? Address1 { get; set; }
    }
}

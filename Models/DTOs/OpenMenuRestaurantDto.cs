namespace FoodOrderingSystem.Models.DTOs.OpenMenu;

public class OpenMenuRestaurantDto
{
    public string RestaurantName { get; set; } = string.Empty;

    public string? BriefDescription { get; set; }

    public string? Address1 { get; set; }

    public string? CityTown { get; set; }

    public string? StateProvince { get; set; }

    public string? PostalCode { get; set; }

    public string? Country { get; set; }

    public string? CuisineTypePrimary { get; set; }

    public string? WebsiteUrl { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace FoodOrderingSystem.Models;

public class OrderAddressViewModel
{
    [Required(ErrorMessage = "Delivery address is required")]
    [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
    public string DeliveryAddress { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "City name cannot exceed 100 characters")]
    public string? DeliveryCity { get; set; }

    [StringLength(100, ErrorMessage = "State name cannot exceed 100 characters")]
    public string? DeliveryState { get; set; }

    [StringLength(20, ErrorMessage = "Zip code cannot exceed 20 characters")]
    public string? DeliveryZipCode { get; set; }

    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Please enter a valid phone number")]
    [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
    public string DeliveryPhone { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Instructions cannot exceed 500 characters")]
    public string? DeliveryInstructions { get; set; }

    [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90")]
    public double? Latitude { get; set; }

    [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180")]
    public double? Longitude { get; set; }
}

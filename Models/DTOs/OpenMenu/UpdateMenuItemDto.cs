using System.ComponentModel.DataAnnotations;

namespace FoodOrderingSystem.Models.DTOs.Menu;

public class UpdateMenuItemDto
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Description { get; set; }

    [Range(typeof(decimal), "0.01", "999999999")]
    public decimal Price { get; set; }

    [Url]
    public string? ImageUrl { get; set; }

    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }

    public bool IsAvailable { get; set; }
}
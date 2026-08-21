using FoodOrderingSystem.Data;
using FoodOrderingSystem.Models.DTOs.Menu;
using FoodOrderingSystem.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingSystem.Controllers.Api;

[ApiController]
[Route("api/menu")]
public class MenuController : ControllerBase
{
    private readonly AppDbContext _context;

    public MenuController(AppDbContext context)
    {
        _context = context;
    }

    // GET: /api/menu
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<MenuItemDto>>> GetMenu()
    {
        var menu = await _context.Foods
            .Include(f => f.Category)
            .Select(f => new MenuItemDto
            {
                Id = f.Id,
                Name = f.Name,
                Description = f.Description,
                Price = f.Price,
                CategoryId = f.CategoryId,
                CategoryName = f.Category != null
                    ? f.Category.Name
                    : null,
                IsAvailable = f.IsAvailable
            })
            .ToListAsync();

        return Ok(menu);
    }

    // GET: /api/menu/1
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<MenuItemDto>> GetMenuItem(int id)
    {
        var food = await _context.Foods
            .Include(f => f.Category)
            .FirstOrDefaultAsync(f => f.Id == id);

        if (food == null)
        {
            return NotFound(new
            {
                message = "Food item not found."
            });
        }

        var result = new MenuItemDto
        {
            Id = food.Id,
            Name = food.Name,
            Description = food.Description,
            Price = food.Price,
            CategoryId = food.CategoryId,
            CategoryName = food.Category?.Name,
            IsAvailable = food.IsAvailable
        };

        return Ok(result);
    }

    // POST: /api/menu
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<MenuItemDto>> CreateMenuItem(
        CreateMenuItemDto dto)
    {
        var categoryExists = await _context.Categories
            .AnyAsync(c => c.Id == dto.CategoryId);

        if (!categoryExists)
        {
            return BadRequest(new
            {
                message = "Invalid category."
            });
        }

        if (dto.Price <= 0)
        {
            return BadRequest(new
            {
                message = "Price must be greater than zero."
            });
        }

        var food = new Food
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            CategoryId = dto.CategoryId,
            IsAvailable = dto.IsAvailable,
            CreatedAt = DateTime.UtcNow
        };

        _context.Foods.Add(food);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetMenuItem),
            new { id = food.Id },
            new MenuItemDto
            {
                Id = food.Id,
                Name = food.Name,
                Description = food.Description,
                Price = food.Price,
                CategoryId = food.CategoryId,
                IsAvailable = food.IsAvailable
            });
    }

    // PUT: /api/menu/1
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateMenuItem(
        int id,
        UpdateMenuItemDto dto)
    {
        var food = await _context.Foods
            .FirstOrDefaultAsync(f => f.Id == id);

        if (food == null)
        {
            return NotFound(new
            {
                message = "Food item not found."
            });
        }

        var categoryExists = await _context.Categories
            .AnyAsync(c => c.Id == dto.CategoryId);

        if (!categoryExists)
        {
            return BadRequest(new
            {
                message = "Invalid category."
            });
        }

        if (dto.Price <= 0)
        {
            return BadRequest(new
            {
                message = "Price must be greater than zero."
            });
        }

        food.Name = dto.Name;
        food.Description = dto.Description;
        food.Price = dto.Price;
        food.CategoryId = dto.CategoryId;
        food.IsAvailable = dto.IsAvailable;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: /api/menu/1
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteMenuItem(int id)
    {
        var food = await _context.Foods
            .FirstOrDefaultAsync(f => f.Id == id);

        if (food == null)
        {
            return NotFound(new
            {
                message = "Food item not found."
            });
        }

        _context.Foods.Remove(food);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
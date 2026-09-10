using FoodOrderingSystem.Data;
using FoodOrderingSystem.Models.Entities;
using FoodOrderingSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingSystem.Repositories.Implementations;

public class FoodRepository : IFoodRepository
{
    private readonly AppDbContext _context;

    public FoodRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Food>> GetAllAsync()
    {
        return await _context.Foods
            .Include(f => f.Category)
            .ToListAsync();
    }

    public async Task<Food?> GetByIdAsync(int id)
    {
        return await _context.Foods
            .Include(f => f.Category)
            .FirstOrDefaultAsync(f => f.Id == id);
    }

    public async Task<bool> ExistsAsync(string name, string restaurantName)
    {
        var normalizedName = name.Trim().ToLower();
        var restaurantMarker = $"Restaurant: {restaurantName.Trim()}";

        return await _context.Foods.AnyAsync(food =>
            food.Name.ToLower() == normalizedName &&
            EF.Functions.Like(food.Description, $"%{restaurantMarker}%"));
    }

    public async Task AddAsync(Food food)
    {
        await _context.Foods.AddAsync(food);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Food food)
    {
        _context.Foods.Update(food);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var food = await _context.Foods.FindAsync(id);

        if (food != null)
        {
            _context.Foods.Remove(food);
            await _context.SaveChangesAsync();
        }
    }
}
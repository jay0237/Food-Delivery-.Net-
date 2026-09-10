using FoodOrderingSystem.Models.Entities;
using FoodOrderingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace FoodOrderingSystem.Controllers;

public class FoodController : Controller
{
    private readonly IFoodService _foodService;
    private readonly ICategoryService _categoryService;
    private readonly IOpenMenuService _openMenuService;

    public FoodController(
        IFoodService foodService,
        ICategoryService categoryService,
        IOpenMenuService openMenuService)
    {
        _foodService = foodService;
        _categoryService = categoryService;
        _openMenuService = openMenuService;
    }

    // GET: /Food
    public async Task<IActionResult> Index()
    {
        var foods = await _foodService.GetAllAsync();

        return View(foods);
    }

    // GET: /Food/Create
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create()
    {
        var categories = await _categoryService.GetAllAsync();

        ViewBag.Categories = categories;

        return View();
    }

    // POST: /Food/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(Food food)
    {
        if (!ModelState.IsValid)
        {
            var categories = await _categoryService.GetAllAsync();

            ViewBag.Categories = categories;

            return View(food);
        }

        await _foodService.AddAsync(food);

        return RedirectToAction(nameof(Index));
    }

    // GET: /Food/Edit/1
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id)
    {
        var food = await _foodService.GetByIdAsync(id);

        if (food == null)
        {
            return NotFound();
        }

        var categories = await _categoryService.GetAllAsync();

        ViewBag.Categories = categories;

        return View(food);
    }

    // POST: /Food/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(Food food)
    {
        if (!ModelState.IsValid)
        {
            var categories = await _categoryService.GetAllAsync();

            ViewBag.Categories = categories;

            return View(food);
        }

        await _foodService.UpdateAsync(food);

        return RedirectToAction(nameof(Index));
    }

    // GET: /Food/Delete/1
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var food = await _foodService.GetByIdAsync(id);

        if (food == null)
        {
            return NotFound();
        }

        return View(food);
    }

    // POST: /Food/Delete/1
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _foodService.DeleteAsync(id);

        return RedirectToAction(nameof(Index));
    }

    // GET: /Food/SearchExternalMenu
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SearchExternalMenu(string? search, string? postalCode, string? country)
    {
        ViewBag.Categories = await _categoryService.GetAllAsync();

        if (string.IsNullOrWhiteSpace(search) || string.IsNullOrWhiteSpace(postalCode) || string.IsNullOrWhiteSpace(country))
        {
            return View(new List<FoodOrderingSystem.Models.DTOs.OpenMenu.OpenMenuItemDto>());
        }

        try
        {
            var results = await _openMenuService.SearchMenuItemsAsync(search.Trim(), postalCode.Trim(), country.Trim());
            return View(results);
        }
        catch (HttpRequestException)
        {
            ModelState.AddModelError(string.Empty, "OpenMenu is unavailable right now. Please try again later.");
        }
        catch (JsonException)
        {
            ModelState.AddModelError(string.Empty, "OpenMenu returned an unexpected response.");
        }
        catch (InvalidOperationException exception)
        {
            ModelState.AddModelError(string.Empty, exception.Message);
        }

        return View(new List<FoodOrderingSystem.Models.DTOs.OpenMenu.OpenMenuItemDto>());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ImportExternalFood(
        FoodOrderingSystem.Models.DTOs.OpenMenu.OpenMenuItemDto item,
        int categoryId,
        decimal localPrice,
        string? search,
        string? postalCode,
        string? country)
    {
        if (string.IsNullOrWhiteSpace(item.MenuItemName))
        {
            ModelState.AddModelError(nameof(item.MenuItemName), "A menu item name is required.");
        }

        if (string.IsNullOrWhiteSpace(item.RestaurantName))
        {
            ModelState.AddModelError(nameof(item.RestaurantName), "A restaurant is required.");
        }

        if (localPrice <= 0)
        {
            ModelState.AddModelError(nameof(localPrice), "Enter a local price greater than zero.");
        }

        var category = await _categoryService.GetByIdAsync(categoryId);
        if (category == null)
        {
            ModelState.AddModelError(nameof(categoryId), "Select an existing category.");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await _categoryService.GetAllAsync();
            ViewBag.ImportItem = item;
            return View("SearchExternalMenu", new List<FoodOrderingSystem.Models.DTOs.OpenMenu.OpenMenuItemDto> { item });
        }

        if (await _foodService.ExistsAsync(item.MenuItemName, item.RestaurantName))
        {
            TempData["Error"] = "This restaurant item has already been imported.";
            return RedirectToSearch(search, postalCode, country);
        }

        var description = string.Join("\n", new[]
        {
            item.MenuItemDescription?.Trim(),
            $"Restaurant: {item.RestaurantName.Trim()}",
            string.IsNullOrWhiteSpace(item.CuisineTypePrimary) ? null : $"Cuisine: {item.CuisineTypePrimary.Trim()}",
            BuildLocation(item)
        }.Where(value => !string.IsNullOrWhiteSpace(value)));

        await _foodService.AddAsync(new Food
        {
            Name = item.MenuItemName.Trim(),
            Description = description,
            Price = localPrice,
            ImageUrl = string.IsNullOrWhiteSpace(item.ImageUrl) ? null : item.ImageUrl.Trim(),
            CategoryId = categoryId,
            IsAvailable = true,
            CreatedAt = DateTime.UtcNow
        });

        TempData["Success"] = $"{item.MenuItemName.Trim()} was imported successfully.";
        return RedirectToSearch(search, postalCode, country);
    }

    private IActionResult RedirectToSearch(string? search, string? postalCode, string? country)
    {
        return RedirectToAction(nameof(SearchExternalMenu), new { search, postalCode, country });
    }

    private static string? BuildLocation(FoodOrderingSystem.Models.DTOs.OpenMenu.OpenMenuItemDto item)
    {
        var location = new[] { item.Address1, item.CityTown, item.StateProvince, item.Country }
            .Where(value => !string.IsNullOrWhiteSpace(value));

        return location.Any() ? $"Location: {string.Join(", ", location)}" : null;
    }
}
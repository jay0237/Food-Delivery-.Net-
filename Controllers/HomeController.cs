using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FoodOrderingSystem.Models;
using FoodOrderingSystem.Services.Interfaces;

namespace FoodOrderingSystem.Controllers;

public class HomeController : Controller
{
    private readonly IFoodService _foodService;
    private readonly ICategoryService _categoryService;

    public HomeController(IFoodService foodService, ICategoryService categoryService)
    {
        _foodService = foodService;
        _categoryService = categoryService;
    }

    public IActionResult Index()
    {
        return ViewAsync();
    }

    private async Task<IActionResult> ViewAsync()
    {
        var foods = (await _foodService.GetAllAsync())
            .Where(food => food.IsAvailable)
            .OrderByDescending(food => food.CreatedAt)
            .ToList();

        var categories = (await _categoryService.GetAllAsync()).ToList();

        return View(new HomeViewModel
        {
            Categories = categories,
            FeaturedFoods = foods.Take(6).ToList(),
            MenuCount = foods.Count
        });
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
